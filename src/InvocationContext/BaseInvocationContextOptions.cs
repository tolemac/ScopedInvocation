using System;
using System.Threading;
using System.Threading.Tasks;

namespace InvocationContext
{
    public class BaseInvocationContextOptions
    {
        public Func<Exception, CancellationToken, Task<bool>>? OnActionExceptionAsync { get; set; }
        public Func<Exception, CancellationToken, Task>? OnInvocationException { get; set; }
        public Func<CancellationToken, Task>? OnActionSuccessAsync { get; set; }
        public Func<CancellationToken, Task>? OnCompleteAsync { get; set; }

        public virtual BaseInvocationContextOptions Clone() => Clone<BaseInvocationContextOptions>();

        public virtual T Clone<T>() where T : BaseInvocationContextOptions, new()
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
