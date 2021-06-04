using Microsoft.Extensions.DependencyInjection;

namespace ScopedInvocation.MicrosoftDi
{
    public class MicrosoftDiScoppedInvocationContext : ScoppedInvocationContext
    {
        public IServiceScope ServiceScope { get; internal set; } = null!;
    }
}
