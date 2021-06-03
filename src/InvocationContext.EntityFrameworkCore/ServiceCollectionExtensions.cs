using InvocationContext.Transactional;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InvocationContext.EntityFrameworkCore
{
    public static class ServiceCollectionExtensions
    {
        public static void AddScopedDbContextTransactionalInvocationContext<TDbContext>(this IServiceCollection sc) where TDbContext : DbContext
        {
            sc.AddScopedTransactionalInvocationContext();

            sc.AddScoped<ITransactionManager, DbContextTransactionManager<TDbContext>>();
        }
    }
}
