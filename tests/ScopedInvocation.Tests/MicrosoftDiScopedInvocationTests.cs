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
            si.ShouldBeOfType<ScopedInvocation>();

            var manager = sp.GetService<IScopedInvocationContextManager>();
            manager.ShouldBeOfType<MicrosoftDiScopedInvocationContextManager>();

            var executionFlag = false;

            await si.InvokeAsync(options => { },
                _ =>
                {
                    executionFlag = true;
                    return Task.CompletedTask;
                });

            
            executionFlag.ShouldBeTrue();
        }


        [Fact]
        public async Task CanUseGenerics()
        {
            var sc = new ServiceCollection();
            sc.AddLogging();
            sc.AddOptions();
            sc.AddScoped<TestService1>();
            sc.AddScoped<TestService2>();
            sc.AddScoped<TestService3>();

            sc.AddMicrosoftDiScopedInvocation();
            await using var sp = sc.BuildServiceProvider();

            var si = sp.GetRequiredService<IScopedInvocation<TestService1, TestService2, TestService3>>();

            await si.InvokeAsync((s1, s2, s3, cancelation) =>
            {
                s1.ShouldNotBeNull();
                s2.ShouldNotBeNull();
                s3.ShouldNotBeNull();
                s1.ShouldBeOfType<TestService1>();
                s2.ShouldBeOfType<TestService2>();
                s3.ShouldBeOfType<TestService3>();
                return Task.CompletedTask;
            });
        }

    }
}
