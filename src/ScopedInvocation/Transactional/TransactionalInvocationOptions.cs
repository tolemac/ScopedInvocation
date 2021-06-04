using System.Data;

namespace ScopedInvocation.Transactional
{
    public class TransactionalInvocationOptions : BaseScopedInvocationOptions
    {
        public bool ReadOnly { get; set; }
        public IsolationLevel IsolationLevel { get; set; }

        public new TransactionalInvocationOptions Clone()
        {
            var result = Clone<TransactionalInvocationOptions>();
            result.ReadOnly = ReadOnly;
            result.IsolationLevel = IsolationLevel;
            return result;
        }
    }
}
