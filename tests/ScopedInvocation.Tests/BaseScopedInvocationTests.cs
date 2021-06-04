using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace ScopedInvocation.Tests
{
    public class BaseScopedInvocationTests
    {
        [Fact]
        public async Task CanPerformCodeUsingScopedInvocation()
        {
            var cm = new ScopedInvocationContextManager();
            var si = new BaseScopedInvocation<BaseScopedInvocationOptions>(cm, null, null);
            var executionFlag = false;
            si.Working.ShouldBeFalse();
            await si.InvokeAsync(sp =>
            {
                si.Working.ShouldBeTrue();
                executionFlag = true;
                return Task.CompletedTask;
            });

            si.Working.ShouldBeFalse();
            executionFlag.ShouldBeTrue();
        }

        [Fact]
        public async Task ScopedInvocationThorwsTheSameExceptionThatStopExecution()
        {
            var cm = new ScopedInvocationContextManager();
            var si = new BaseScopedInvocation<BaseScopedInvocationOptions>(cm, null, null);
            
            (await Should.ThrowAsync<Exception>(async () =>
            {
                await si.InvokeAsync(sp => throw new Exception("Test exception"));
            })).Message.ShouldBe("Test exception");
        }
    }
}
