using System;
using Microsoft.Extensions.DependencyInjection;

namespace ScopedInvocation.MicrosoftDi
{
    public class MicrosoftDiScopedInvocationContextManager : IScopedInvocationContextManager
    {
        private readonly IServiceProvider _serviceProvider;

        public MicrosoftDiScopedInvocationContextManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ScoppedInvocationContext InitializeScope()
        {
            var serviceScope = _serviceProvider.CreateScope();
            return new MicrosoftDiScoppedInvocationContext
            {
                ServiceScope = serviceScope,
                ServiceProvider = serviceScope.ServiceProvider

            };
        }

        public void FinallizeScope(ScoppedInvocationContext context)
        {
            ((MicrosoftDiScoppedInvocationContext) context).ServiceScope.Dispose();
        }
    }
}
