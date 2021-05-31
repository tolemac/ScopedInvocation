using System;
using Microsoft.Extensions.DependencyInjection;

namespace InvocationContext.MicrosoftDi
{
    public class MicrosoftDiInvocationContextDataManager : IInvocationContextDataManager
    {
        private readonly IServiceProvider _serviceProvider;

        public MicrosoftDiInvocationContextDataManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public InvocationContextData InitializeContext()
        {
            var serviceScope = _serviceProvider.CreateScope();
            return new MicrosoftDiInvocationContextData
            {
                ServiceScope = serviceScope,
                ServiceProvider = serviceScope.ServiceProvider

            };
        }

        public void FinallizeContext(InvocationContextData data)
        {
            ((MicrosoftDiInvocationContextData) data).ServiceScope.Dispose();
        }
    }
}
