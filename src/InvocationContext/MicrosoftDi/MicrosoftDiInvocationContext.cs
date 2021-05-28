using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InvocationContext.MicrosoftDi
{
    public class MicrosoftDiInvocationContext : BaseInvocationContext<MicrosoftDiInvocationContextData, BaseInvocationContextOptions>
    {
        private readonly IServiceProvider _serviceProvider;

        public MicrosoftDiInvocationContext(IServiceProvider serviceProvider, IOptions<BaseInvocationContextOptions>? defaultOptions, ILogger<MicrosoftDiInvocationContext> logger) 
            : base(defaultOptions, logger)
        {
            _serviceProvider = serviceProvider;
        }

        protected override MicrosoftDiInvocationContextData InitializeContext()
        {
            var serviceScope = _serviceProvider.CreateScope();
            return new MicrosoftDiInvocationContextData
            {
                ServiceScope = serviceScope,
                ServiceProvider = serviceScope.ServiceProvider

            };
        }

        protected override void FinallizeContext(MicrosoftDiInvocationContextData data)
        {
            data.ServiceScope.Dispose();
            base.FinallizeContext(data);
        }
    }
}
