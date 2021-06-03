using System;
using System.Threading.Tasks;
using InvocationContext.Transactional;
using Xunit;
using Moq;
using Shouldly;

namespace InvocationContext.Tests
{
    public class TransactionalInvocationContextTests
    {
        [Fact]
        public async Task CommitIsCalled()
        {
            var mock = new Mock<ITransactionManager>();
            var dm = new InvocationContextDataManager();
            var ic = new TransactionalInvocationContext(dm, null, null, mock.Object);

            await ic.InvokeAsync(sp => Task.CompletedTask);

            mock.Verify(tm => tm.StartTransaction(It.IsAny<TransactionalInvocationContextOptions>(), It.IsAny<InvocationContextData>()), Times.Once());
            mock.Verify(tm => tm.Commit(), Times.Once());
            mock.Verify(tm => tm.Rollback(), Times.Never);
        }

        [Fact]
        public async Task RollbackIsCalled()
        {
            var mock = new Mock<ITransactionManager>();
            var dm = new InvocationContextDataManager();
            var ic = new TransactionalInvocationContext(dm, null, null, mock.Object);

            await Should.ThrowAsync<Exception>(async () => { await ic.InvokeAsync(sp => throw new Exception()); });

            mock.Verify(tm => tm.StartTransaction(It.IsAny<TransactionalInvocationContextOptions>(), It.IsAny<InvocationContextData>()), Times.Once);
            mock.Verify(tm => tm.Commit(), Times.Never);
            mock.Verify(tm => tm.Rollback(), Times.Once);
        }

    }
}
