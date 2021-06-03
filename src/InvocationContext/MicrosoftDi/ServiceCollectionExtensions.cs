using Microsoft.Extensions.DependencyInjection;

namespace InvocationContext.MicrosoftDi
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMicrosoftDiInvocationContext(this IServiceCollection sc)
        {
            sc.AddSingleton<IInvocationContext, InvocationContext>();
            sc.AddSingleton<IInvocationContextDataManager, MicrosoftDiInvocationContextDataManager>();
        }
    }
}
