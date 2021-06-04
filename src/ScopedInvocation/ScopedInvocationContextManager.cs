namespace ScopedInvocation
{
    public interface IScopedInvocationContextManager
    {
        ScoppedInvocationContext InitializeScope();
        void FinallizeScope(ScoppedInvocationContext context);
    }

    public class ScopedInvocationContextManager : IScopedInvocationContextManager
    {
        public ScoppedInvocationContext InitializeScope()
        {
            return new ScoppedInvocationContext();
        }

        public void FinallizeScope(ScoppedInvocationContext context)
        {
        }
    }
}
