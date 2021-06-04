using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ScopedInvocation.MicrosoftDi;
using Shouldly;
using Xunit;

namespace ScopedInvocation.Tests
{
    public class MicrosoftDiScopedInvocationTests
    {
        [Fact]
        public async Task CanUseMicrosoftDi()
        {
            var sc = new ServiceCollection();
            sc.AddLogging();
            sc.AddOptions();
            
            sc.AddMicrosoftDiScopedInvocation();
            await using var sp = sc.BuildServiceProvider();

            var si = sp.GetService<IScopedInvocation>();
            si.ShouldBeOfType<BaseScopedInvocation>();

            var manager = sp.GetService<IScopedInvocationContextManager>();
            manager.ShouldBeOfType<MicrosoftDiScopedInvocationContextManager>();

            var executionFlag = false;

            await si.InvokeAsync(options => { },
                sp =>
                {
                    executionFlag = true;
                    return Task.CompletedTask;
                });

            
            executionFlag.ShouldBeTrue();
        }
    }
}
