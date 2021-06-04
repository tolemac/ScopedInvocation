using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ScopedInvocation.Transactional;

namespace ScopedInvocation.EntityFrameworkCore
{
    public static class ServiceCollectionExtensions
    {
        public static void AddScopedDbContextTransactionalInvocation<TDbContext>(this IServiceCollection sc) where TDbContext : DbContext
        {
            sc.AddScopedTransactionalInvocation();
            sc.AddScoped<ITransactionManager, ScopedDbContextTransactionManager<TDbContext>>();
        }
    }
}
