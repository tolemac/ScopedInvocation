using System;
using System.Threading.Tasks;
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

    }
}
