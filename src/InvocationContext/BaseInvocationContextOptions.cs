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
        public BaseInvocationContextOptions Clone()
        {
            return new BaseInvocationContextOptions
            {
                OnActionExceptionAsync = OnActionExceptionAsync,
                OnActionSuccessAsync = OnActionSuccessAsync,
                OnCompleteAsync = OnCompleteAsync,
                OnInvocationException = OnInvocationException
            };
        }

    }
}
