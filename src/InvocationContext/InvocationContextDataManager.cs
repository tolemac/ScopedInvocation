namespace InvocationContext
{
    public interface IInvocationContextDataManager
    {
        InvocationContextData InitializeContext();
        void FinallizeContext(InvocationContextData data);
    }

    public class InvocationContextDataManager : IInvocationContextDataManager
    {
        public InvocationContextData InitializeContext()
        {
            return new InvocationContextData();
        }

        public void FinallizeContext(InvocationContextData data)
        {
        }
    }
}
