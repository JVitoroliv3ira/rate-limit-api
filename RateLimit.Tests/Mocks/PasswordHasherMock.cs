using Moq;
using RateLimit.Application.Interfaces.Services;

namespace RateLimit.Tests.Mocks;

public static class PasswordHasherMock
{
    public static Mock<IPasswordHasher> Create(string hash = "hashedPassword")
    {
        var mock = new Mock<IPasswordHasher>();

        mock.Setup(x => x.Hash(It.IsAny<string>())).Returns(hash);

        return mock;
    }
}