using System.Data;

namespace InvocationContext.Transactional
{
    public class TransactionalInvocationContextOptions : BaseInvocationContextOptions
    {
        public bool ReadOnly { get; set; }
        public IsolationLevel IsolationLevel { get; set; }

        public new TransactionalInvocationContextOptions Clone()
        {
            var result = Clone<TransactionalInvocationContextOptions>();
            result.ReadOnly = ReadOnly;
            result.IsolationLevel = IsolationLevel;
            return result;
        }
    }
}
