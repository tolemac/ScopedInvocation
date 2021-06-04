using System;
using System.Threading;
using System.Threading.Tasks;

namespace ScopedInvocation.Transactional
{
    public interface ITransactionalInvocation
    {
        bool Working { get; }

        Task InvokeAsync(Func<IServiceProvider, Task> action, CancellationToken cancellation = default);
        Task InvokeAsync(Action<TransactionalInvocationOptions>? optionsAction,
            Func<IServiceProvider, Task> action, CancellationToken cancellation = default);

        Task ReadOnlyInvokeAsync(Func<IServiceProvider, Task> action, CancellationToken cancellation = default);
        Task ReadOnlyInvokeAsync(Action<TransactionalInvocationOptions>? optionsAction,
            Func<IServiceProvider, Task> action, CancellationToken cancellation = default);
    }
}
