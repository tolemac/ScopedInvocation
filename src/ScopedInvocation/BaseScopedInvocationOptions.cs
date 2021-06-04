namespace ScopedInvocation
{
    public class BaseScopedInvocationOptions
    {
        public virtual BaseScopedInvocationOptions Clone() => Clone<BaseScopedInvocationOptions>();

        public virtual T Clone<T>() where T : BaseScopedInvocationOptions, new()
        {
            return new T();
        }
    }
}
