using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InvocationContext.Transactional
{
    public class TransactionalInvocationContext : BaseInvocationContext<TransactionalInvocationContextOptions>, ITransactionalInvocation
    {
        private readonly ITransactionManager _transactionManager;

        public TransactionalInvocationContext(IInvocationContextDataManager dataManager, 
            IOptions<TransactionalInvocationContextOptions>? defaultOptions, 
            ILogger<BaseInvocationContext<TransactionalInvocationContextOptions>>? logger,
            ITransactionManager transactionManager) : base(dataManager, defaultOptions, logger)
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

        public Task ReadOnlyInvokeAsync(Action<TransactionalInvocationContextOptions>? optionsAction, Func<IServiceProvider, Task> action, CancellationToken cancellation = default)
        {
            return base.InvokeAsync(opt =>
            {
                optionsAction?.Invoke(opt);
                opt.ReadOnly = true;
            }, action, cancellation);
        }

        protected override void BeforeActionIvocation(TransactionalInvocationContextOptions options, InvocationContextData data)
        {
            base.BeforeActionIvocation(options, data);
            _transactionManager.StartTransaction(options, data);
        }

        protected override void AfterActionSuccessfulInvocation(TransactionalInvocationContextOptions options, InvocationContextData data)
        {
            base.AfterActionSuccessfulInvocation(options, data);
            _transactionManager.Commit();
        }

        protected override void OnActionException(TransactionalInvocationContextOptions options, InvocationContextData data)
        {
            base.OnActionException(options, data);
            _transactionManager.Rollback();
        }
    }
}
