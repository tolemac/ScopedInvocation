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
            
            BeforeActionIvocation(options, context);
            try
            {
                Working = true;
                await action.Invoke(context.ServiceProvider);
                
                AfterActionSuccessfulInvocation(options, context);
            }
            catch (Exception ex)
            {
                OnActionException(options, context);

                _logger?.LogError(ex, "Error on scoped invocation");
                throw;
            }
            finally
            {
                Working = false;
                _contextManager.FinallizeScope(context);
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
