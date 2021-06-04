using System;

namespace ScopedInvocation
{
    public class ScoppedInvocationContext
    {
        public IServiceProvider ServiceProvider { get; internal set; } = null!;
    }
}
