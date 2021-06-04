using System.Data;

namespace ScopedInvocation.Transactional
{
    public class ScopedConnectionTransactionManager<TConnection> : ITransactionManager
    where TConnection : IDbConnection
    {
        private readonly TConnection _connection;
        private IDbTransaction? _trans;

        public ScopedConnectionTransactionManager(TConnection connection)
        {
            _connection = connection;
        }

        public void StartTransaction(TransactionalInvocationOptions options, ScoppedInvocationContext context)
        {
            _trans = _connection.BeginTransaction(options.IsolationLevel);
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
