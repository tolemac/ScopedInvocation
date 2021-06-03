namespace InvocationContext.Transactional
{
    public interface ITransactionManager
    {
        void StartTransaction(TransactionalInvocationContextOptions options, InvocationContextData data);
        void Commit();
        void Rollback();
    }
}
