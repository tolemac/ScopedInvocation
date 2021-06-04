using System.Data;
using Microsoft.Extensions.DependencyInjection;
using ScopedInvocation.MicrosoftDi;
using ScopedInvocation.Transactional;

namespace ScopedInvocation
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMicrosoftDiScopedInvocation(this IServiceCollection sc)
        {
            sc.AddSingleton<IScopedInvocation, BaseScopedInvocation>();
            sc.AddSingleton<IScopedInvocationContextManager, MicrosoftDiScopedInvocationContextManager>();
        }

        public static void AddScopedTransactionalScopedInvocation(this IServiceCollection sc)
        {
            sc.AddScoped<ITransactionalInvocation, TransactionalInvocation>();
        }

        public static void AddScopedConnectionTransactionalInvocation<TConnection>(this IServiceCollection sc) 
            where TConnection : class, IDbConnection, new()
        {
            sc.AddScopedTransactionalScopedInvocation();
            sc.AddScoped<ITransactionManager, ScopedConnectionTransactionManager<TConnection>>();
        }
    }
}
