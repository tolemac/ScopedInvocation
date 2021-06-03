using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InvocationContext
{
    public class InvocationContext : BaseInvocationContext<BaseInvocationContextOptions>, IInvocationContext
    {
        public InvocationContext(IInvocationContextDataManager dataManager, 
            IOptions<BaseInvocationContextOptions>? defaultOptions, 
            ILogger<BaseInvocationContext<BaseInvocationContextOptions>>? logger) 
            : base(dataManager, defaultOptions, logger)
        {
        }
    }
}
