using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ScopedInvocation
{
    public class BaseScopedInvocation : BaseScopedInvocation<BaseScopedInvocationOptions>, IScopedInvocation
    {
        public BaseScopedInvocation(IScopedInvocationContextManager contextManager,
            IOptions<BaseScopedInvocationOptions>? defaultOptions,
            ILogger<BaseScopedInvocation<BaseScopedInvocationOptions>>? logger)
            : base(contextManager, defaultOptions, logger)
        {
        }
    }

    public class BaseScopedInvocation<TScopedInvocationOptions>  
        where TScopedInvocationOptions : BaseScopedInvocationOptions, new()
    {
        private readonly IScopedInvocationContextManager _contextManager;
        private readonly ILogger? _logger;
        private readonly TScopedInvocationOptions _defaultOptions;

        public bool Working { get; private set; }

        public BaseScopedInvocation(IScopedInvocationContextManager contextManager, 
            IOptions<TScopedInvocationOptions>? defaultOptions, ILogger<BaseScopedInvocation<TScopedInvocationOptions>>? logger)
        {
            _contextManager = contextManager;
            _logger = logger;
            _defaultOptions = defaultOptions?.Value ?? new TScopedInvocationOptions();
        }

        public virtual async Task InvokeAsync(Func<IServiceProvider, Task> action, CancellationToken cancellation = default)
        {
            await InvokeAsync(_ => {  }, action, cancellation);
        }

        public virtual async Task InvokeAsync(Action<TScopedInvocationOptions>? optionsAction,
            Func<IServiceProvider, Task> action, CancellationToken cancellation = default)
        {
            var options = _defaultOptions.Clone<TScopedInvocationOptions>();
            optionsAction?.Invoke(options);

            await InvokeAsync(options, action, cancellation);
        }

        protected virtual async Task InvokeAsync(TScopedInvocationOptions options,
            Func<IServiceProvider, Task> action, CancellationToken cancellation = default)
        {
            var context = _contextManager.InitializeScope();

            Exception? exceptionOnOnActionSuccess = null;

            try
            {
                BeforeActionIvocation(options, context);
                try
                {
                    Working = true;
                    await action.Invoke(context.ServiceProvider);
                    
                    AfterActionSuccessfulInvocation(options, context);

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
                    OnActionException(options, context);

                    _logger?.LogError(ex, "Error on scoped invocation");
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
                                "An error occurred calling OnActionExceptionAsync after a first error occurred on scoped invocation",
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
                        _contextManager.FinallizeScope(context);
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

        protected virtual void BeforeActionIvocation(TScopedInvocationOptions options, ScoppedInvocationContext context)
        {
        }
        protected virtual void AfterActionSuccessfulInvocation(TScopedInvocationOptions options, ScoppedInvocationContext context)
        {
        }
        protected virtual void OnActionException(TScopedInvocationOptions options, ScoppedInvocationContext context)
        {
        }
    }
}
