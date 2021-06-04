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
            sc.AddSingleton<IScopedInvocation, ScopedInvocation>();
            sc.AddScopedInvocationGenerics();
            sc.AddSingleton<IScopedInvocationContextManager, MicrosoftDiScopedInvocationContextManager>();
        }

        public static void AddScopedTransactionalInvocation(this IServiceCollection sc)
        {
            sc.AddScoped<ITransactionalInvocation, TransactionalInvocation>();
            sc.AddTransactionalInvocationGenerics();
        }

        public static void AddScopedConnectionTransactionalInvocation<TConnection>(this IServiceCollection sc) 
            where TConnection : class, IDbConnection, new()
        {
            sc.AddScopedTransactionalInvocation();
            sc.AddScoped<ITransactionManager, ScopedConnectionTransactionManager<TConnection>>();
        }
    }
}
