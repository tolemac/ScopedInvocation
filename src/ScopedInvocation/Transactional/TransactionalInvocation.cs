using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ScopedInvocation.Transactional
{
    public class TransactionalInvocation : BaseScopedInvocation<TransactionalInvocationOptions>, ITransactionalInvocation
    {
        private readonly ITransactionManager _transactionManager;

        public TransactionalInvocation(IScopedInvocationContextManager contextManager, 
            IOptions<TransactionalInvocationOptions>? defaultOptions, 
            ILogger<BaseScopedInvocation<TransactionalInvocationOptions>>? logger,
            ITransactionManager transactionManager) : base(contextManager, defaultOptions, logger)
        {
            _transactionManager = transactionManager;
        }

        public Task ReadOnlyInvokeAsync(Func<IServiceProvider, Task> action, CancellationToken cancellation = default)
        {
            return base.InvokeAsync(opt =>
            {
                opt.ReadOnly = true;
            }, action, cancellation);
        }

        public Task ReadOnlyInvokeAsync(Action<TransactionalInvocationOptions>? optionsAction, Func<IServiceProvider, Task> action, CancellationToken cancellation = default)
        {
            return base.InvokeAsync(opt =>
            {
                optionsAction?.Invoke(opt);
                opt.ReadOnly = true;
            }, action, cancellation);
        }

        protected override void BeforeActionIvocation(TransactionalInvocationOptions options, ScoppedInvocationContext context)
        {
            base.BeforeActionIvocation(options, context);
            _transactionManager.StartTransaction(options, context);
        }

        protected override void AfterActionSuccessfulInvocation(TransactionalInvocationOptions options, ScoppedInvocationContext context)
        {
            base.AfterActionSuccessfulInvocation(options, context);
            _transactionManager.Commit();
        }

        protected override void OnActionException(TransactionalInvocationOptions options, ScoppedInvocationContext context)
        {
            base.OnActionException(options, context);
            _transactionManager.Rollback();
        }
    }
}
