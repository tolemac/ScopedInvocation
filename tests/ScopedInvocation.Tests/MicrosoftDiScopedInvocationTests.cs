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

            var ic = sp.GetService<IScopedInvocation>();
            ic.ShouldBeOfType<BaseScopedInvocation>();

            var manager = sp.GetService<IScopedInvocationContextManager>();
            manager.ShouldBeOfType<MicrosoftDiScopedInvocationContextManager>();

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
