using Microsoft.Extensions.DependencyInjection;

namespace InvocationContext.MicrosoftDi
{
    public class MicrosoftDiInvocationContextData : InvocationContextData
    {
        public IServiceScope ServiceScope { get; internal set; } = null!;
    }
}
