using System;
using System.Threading;
using System.Threading.Tasks;

namespace InvocationContext.Transactional
{
    public interface ITransactionalInvocation
    {
        bool Working { get; }

        Task InvokeAsync(Func<IServiceProvider, Task> action, CancellationToken cancellation = default);
        Task InvokeAsync(Action<TransactionalInvocationContextOptions>? optionsAction,
            Func<IServiceProvider, Task> action, CancellationToken cancellation = default);

        Task ReadOnlyInvokeAsync(Func<IServiceProvider, Task> action, CancellationToken cancellation = default);
        Task ReadOnlyInvokeAsync(Action<TransactionalInvocationContextOptions>? optionsAction,
            Func<IServiceProvider, Task> action, CancellationToken cancellation = default);
    }
}
