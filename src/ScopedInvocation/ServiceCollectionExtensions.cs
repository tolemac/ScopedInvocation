using System.Data;
using ScopedInvocation;
using ScopedInvocation.MicrosoftDi;
using ScopedInvocation.Transactional;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMicrosoftDiScopedInvocation(this IServiceCollection sc)
        {
            sc.AddSingleton<IScopedInvocation, ScopedInvocation.ScopedInvocation>();
            sc.AddScopedInvocationGenerics();
            sc.AddSingleton<IScopedInvocationContextManager, MicrosoftDiScopedInvocationContextManager>();
            return sc;
        }

        public static IServiceCollection AddScopedTransactionalInvocation(this IServiceCollection sc)
        {
            sc.AddScoped<ITransactionalInvocation, TransactionalInvocation>();
            sc.AddTransactionalInvocationGenerics();
            return sc;
        }

        public static IServiceCollection AddScopedConnectionTransactionalInvocation<TConnection>(this IServiceCollection sc) 
            where TConnection : class, IDbConnection, new()
        {
            sc.AddScopedTransactionalInvocation();
            sc.AddScoped<ITransactionManager, ScopedConnectionTransactionManager<TConnection>>();
            return sc;
        }
    }
}
