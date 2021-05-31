using Microsoft.Extensions.DependencyInjection;

namespace InvocationContext.MicrosoftDi
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMicrosoftDiInvocationContext(this IServiceCollection sc)
        {
            sc.AddSingleton<IInvocationContext,
                BaseInvocationContext<BaseInvocationContextOptions>>();
            sc.AddSingleton<IInvocationContextDataManager, MicrosoftDiInvocationContextDataManager>();
        }
    }
}
