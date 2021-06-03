using InvocationContext.MicrosoftDi;
using InvocationContext.Transactional;
using Microsoft.Extensions.DependencyInjection;

namespace InvocationContext
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMicrosoftDiInvocationContext(this IServiceCollection sc)
        {
            sc.AddSingleton<IInvocationContext, BaseInvocationContext>();
            sc.AddSingleton<IInvocationContextDataManager, MicrosoftDiInvocationContextDataManager>();
        }

        public static void AddScopedTransactionalInvocationContext(this IServiceCollection sc)
        {
            sc.AddScoped<ITransactionalInvocation, TransactionalInvocationContext>();
        }
    }
}
