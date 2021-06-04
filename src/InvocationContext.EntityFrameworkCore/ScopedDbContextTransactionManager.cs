using InvocationContext.Transactional;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace InvocationContext.EntityFrameworkCore
{
    public class ScopedDbContextTransactionManager<TDbContext> : ITransactionManager
        where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;
        private IDbContextTransaction? _trans;

        public ScopedDbContextTransactionManager(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void StartTransaction(TransactionalInvocationContextOptions options, InvocationContextData data)
        {
            _trans = _dbContext.Database.BeginTransaction(options.IsolationLevel);
        }

        public void Commit()
        {
            _trans!.Commit();
        }

        public void Rollback()
        {
            _trans!.Rollback();
        }
    }
}
