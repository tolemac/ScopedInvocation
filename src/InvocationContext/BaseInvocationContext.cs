using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InvocationContext
{
    public class BaseInvocationContext<TInvocationContextData, TInvocationContextOptions>
        where TInvocationContextOptions : BaseInvocationContextOptions, new()
        where TInvocationContextData : InvocationContextData, new()
    {
        private readonly ILogger? _logger;
        private readonly TInvocationContextOptions _defaultOptions;

        public bool Working { get; private set; }

        public BaseInvocationContext(IOptions<TInvocationContextOptions>? defaultOptions, ILogger? logger)
        {
            _logger = logger;
            _defaultOptions = defaultOptions?.Value ?? new TInvocationContextOptions();
        }

        protected virtual TInvocationContextData InitializeContext()
        {
            return new TInvocationContextData();
        }

        protected virtual void FinallizeContext(TInvocationContextData data)
        {
        }

        public virtual async Task InvokeAsync(Action<BaseInvocationContextOptions>? optionsAction,
            Func<IServiceProvider, Task> action, CancellationToken cancellation = default)
        {
            var options = _defaultOptions.Clone();
            optionsAction?.Invoke(options);

            var contextData = InitializeContext();

            Exception? exceptionOnOnActionSuccess = null;

            try
            {
                try
                {
                    Working = true;
                    await action.Invoke(contextData.ServiceProvider);

                    if (options.OnActionSuccessAsync is not null)
                    {
                        try
                        {
                            await options.OnActionSuccessAsync.Invoke(cancellation);
                        }
                        catch (Exception ex)
                        {
                            exceptionOnOnActionSuccess = ex;
                            _logger?.LogError(ex, "Error calling OnActionSuccessAsync");
                            throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Error on invocation context");
                    if (exceptionOnOnActionSuccess is not null)
                        throw;

                    var rethrow = true;
                    if (options.OnActionExceptionAsync is not null)
                    {
                        try
                        {
                            rethrow = await options.OnActionExceptionAsync.Invoke(ex, cancellation);
                        }
                        catch (Exception innerEx)
                        {
                            _logger?.LogError(innerEx, "Error calling OnActionExceptionAsync");
                            throw new AggregateException(
                                "An error occurred calling OnActionExceptionAsync after a first error occurred on invocation context",
                                ex, innerEx);
                        }
                    }

                    if (rethrow)
                    {
                        throw;
                    }
                }
                finally
                {
                    Working = false;
                    try
                    {
                        if (options.OnCompleteAsync is not null)
                        {
                            await options.OnCompleteAsync.Invoke(cancellation);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "Error calling OnCompleteAsync");
                        throw;
                    }
                    finally
                    {
                        FinallizeContext(contextData);
                    }
                }
            }
            catch (Exception outterException)
            {
                if (options.OnInvocationException is not null)
                {
                    await options.OnInvocationException.Invoke(outterException, cancellation);
                }
                throw;
            }

        }
    }
}
