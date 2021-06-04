using System.Data;

namespace InvocationContext.Transactional
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

        public void StartTransaction(TransactionalInvocationContextOptions options, InvocationContextData data)
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
