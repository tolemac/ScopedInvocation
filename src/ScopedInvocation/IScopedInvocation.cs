using System;
using System.Threading;
using System.Threading.Tasks;

namespace ScopedInvocation
{
    public interface IScopedInvocation
    {
        bool Working { get; }

        Task InvokeAsync(Func<IServiceProvider, Task> action, CancellationToken cancellation = default);
        Task InvokeAsync(Action<BaseScopedInvocationOptions>? optionsAction,
            Func<IServiceProvider, Task> action, CancellationToken cancellation = default);
    }
}
