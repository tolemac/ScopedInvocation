using System.Threading.Tasks;
using InvocationContext.MicrosoftDi;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace InvocationContext.Tests
{
    public class MicrosoftDiInvocationContextTests
    {
        [Fact]
        public async Task CanUseMicrosoftDi()
        {
            var sc = new ServiceCollection();
            sc.AddLogging();
            sc.AddOptions();
            
            sc.AddMicrosoftDiInvocationContext();
            await using var sp = sc.BuildServiceProvider();

            var ic = sp.GetService<IInvocationContext>();
            ic.ShouldBeOfType<InvocationContext>();

            var manager = sp.GetService<IInvocationContextDataManager>();
            manager.ShouldBeOfType<MicrosoftDiInvocationContextDataManager>();

            var executionFlag = false;

            await ic.InvokeAsync(options => { },
                sp =>
                {
                    executionFlag = true;
                    return Task.CompletedTask;
                });

            
            executionFlag.ShouldBeTrue();
        }
    }
}
