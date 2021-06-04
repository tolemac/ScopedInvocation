namespace ScopedInvocation.Transactional
{
    public interface ITransactionManager
    {
        void StartTransaction(TransactionalInvocationOptions options, ScoppedInvocationContext context);
        void Commit();
        void Rollback();
    }
}
