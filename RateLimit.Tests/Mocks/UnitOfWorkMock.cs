using Moq;
using RateLimit.Application.Interfaces.Core;

namespace RateLimit.Tests.Mocks;

public static class UnitOfWorkMock
{
    public static Mock<IUnitOfWork> Create()
    {
        var mock = new Mock<IUnitOfWork>();

        mock.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        return mock;
    }
}