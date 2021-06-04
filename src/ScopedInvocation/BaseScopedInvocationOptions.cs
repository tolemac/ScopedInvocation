using System;
using System.Threading;
using System.Threading.Tasks;

namespace ScopedInvocation
{
    public class BaseScopedInvocationOptions
    {
        public Func<Exception, CancellationToken, Task<bool>>? OnActionExceptionAsync { get; set; }
        public Func<Exception, CancellationToken, Task>? OnInvocationException { get; set; }
        public Func<CancellationToken, Task>? OnActionSuccessAsync { get; set; }
        public Func<CancellationToken, Task>? OnCompleteAsync { get; set; }

        public virtual BaseScopedInvocationOptions Clone() => Clone<BaseScopedInvocationOptions>();

        public virtual T Clone<T>() where T : BaseScopedInvocationOptions, new()
        {
            return new T
            {
                OnActionExceptionAsync = OnActionExceptionAsync,
                OnActionSuccessAsync = OnActionSuccessAsync,
                OnCompleteAsync = OnCompleteAsync,
                OnInvocationException = OnInvocationException
            };
        }
    }
}
