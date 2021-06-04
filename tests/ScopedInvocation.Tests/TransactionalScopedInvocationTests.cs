using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ScopedInvocation.Transactional;
using Shouldly;
using Xunit;

namespace ScopedInvocation.Tests
{
    public class TransactionalScopedInvocationTests
    {
        [Fact]
        public async Task CommitIsCalled()
        {
            var mock = new Mock<ITransactionManager>();
            var dm = new ScopedInvocationContextManager();
            var ic = new TransactionalInvocation(dm, null, null, mock.Object);

            await ic.InvokeAsync(sp => Task.CompletedTask);

            mock.Verify(tm => tm.StartTransaction(It.IsAny<TransactionalInvocationOptions>(), It.IsAny<ScoppedInvocationContext>()), Times.Once());
            mock.Verify(tm => tm.Commit(), Times.Once());
            mock.Verify(tm => tm.Rollback(), Times.Never);
        }

        [Fact]
        public async Task RollbackIsCalled()
        {
            var mock = new Mock<ITransactionManager>();
            var dm = new ScopedInvocationContextManager();
            var ic = new TransactionalInvocation(dm, null, null, mock.Object);

            await Should.ThrowAsync<Exception>(async () => { await ic.InvokeAsync(sp => throw new Exception()); });

            mock.Verify(tm => tm.StartTransaction(It.IsAny<TransactionalInvocationOptions>(), It.IsAny<ScoppedInvocationContext>()), Times.Once);
            mock.Verify(tm => tm.Commit(), Times.Never);
            mock.Verify(tm => tm.Rollback(), Times.Once);
        }

        [Fact]
        public async Task CanUseTransactionalGenerics()
        {
            var sc = new ServiceCollection();
            sc.AddLogging();
            sc.AddOptions();
            sc.AddScoped<TestService1>();
            sc.AddScoped<TestService2>();
            sc.AddScoped<TestService3>();

            var mock = new Mock<ITransactionManager>();
            sc.AddScoped(sp => mock.Object);

            sc.AddMicrosoftDiScopedInvocation();
            sc.AddScopedTransactionalInvocation();
            await using var sp = sc.BuildServiceProvider();

            var ti = sp.GetRequiredService<ITransactionalInvocation<TestService1, TestService2, TestService3>>();

            await ti.InvokeAsync((s1, s2, s3, cancelation) =>
            {
                s1.ShouldNotBeNull();
                s2.ShouldNotBeNull();
                s3.ShouldNotBeNull();
                s1.ShouldBeOfType<TestService1>();
                s2.ShouldBeOfType<TestService2>();
                s3.ShouldBeOfType<TestService3>();
                return Task.CompletedTask;
            });

            mock.Verify(tm => tm.StartTransaction(It.IsAny<TransactionalInvocationOptions>(), It.IsAny<ScoppedInvocationContext>()), Times.Once);
            mock.Verify(tm => tm.Commit(), Times.Once);
            mock.Verify(tm => tm.Rollback(), Times.Never);
        }

    }
}
