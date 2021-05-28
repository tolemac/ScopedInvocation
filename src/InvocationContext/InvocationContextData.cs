using System;

namespace InvocationContext
{
    public class InvocationContextData
    {
        public IServiceProvider ServiceProvider { get; internal set; } = null!;
    }
}
