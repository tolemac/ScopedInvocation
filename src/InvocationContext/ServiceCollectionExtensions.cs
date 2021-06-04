using System.Data;
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

        public static void AddScopedConnectionTransactionManager<TConnection>(this IServiceCollection sc) 
            where TConnection : class, IDbConnection, new()
        {
            sc.AddScopedTransactionalInvocationContext();
            sc.AddScoped<ITransactionManager, ScopedConnectionTransactionManager<TConnection>>();
        }
    }
}
