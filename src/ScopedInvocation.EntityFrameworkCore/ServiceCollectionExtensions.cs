using Microsoft.EntityFrameworkCore;
using ScopedInvocation.EntityFrameworkCore;
using ScopedInvocation.Transactional;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddScopedDbContextTransactionalInvocation<TDbContext>(this IServiceCollection sc) where TDbContext : DbContext
        {
            sc.AddScopedTransactionalInvocation();
            sc.AddScoped<ITransactionManager, ScopedDbContextTransactionManager<TDbContext>>();
            return sc;
        }
    }
}
