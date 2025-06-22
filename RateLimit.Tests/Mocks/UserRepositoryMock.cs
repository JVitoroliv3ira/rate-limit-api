using System.Linq.Expressions;
using Moq;
using RateLimit.Application.Interfaces.Core;
using RateLimit.Domain.Entities;

namespace RateLimit.Tests.Mocks;

public static class UserRepositoryMock
{
    public static Mock<IRepository<User>> Create()
    {
        var mock = new Mock<IRepository<User>>();

        mock.Setup(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        mock.Setup(x => x.ExistByAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        return mock;
    }
}