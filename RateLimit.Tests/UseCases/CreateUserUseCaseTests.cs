using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using RateLimit.Application.Dtos.Commands.Users;
using RateLimit.Application.Interfaces.Core;
using RateLimit.Application.Interfaces.Services;
using RateLimit.Application.UseCases.Users;
using RateLimit.Domain.Entities;

namespace RateLimit.Tests.UseCases;

public class CreateUserUseCaseTests
{
    private readonly Mock<IRepository<User>> _userRepositoryMock = new();
    private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly CreateUserUseCase _sut;

    public CreateUserUseCaseTests()
    {
        _sut = new CreateUserUseCase(
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _passwordHasherMock.Object
        );
    }

    [Fact]
    public async Task Should_Return_Error_If_Email_Is_Already_In_Use()
    {
        var command = NewUserCommand();

        _userRepositoryMock
            .Setup(x => x.ExistByAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _sut.ExecuteAsync(command, CancellationToken.None);

        result.Match(
            error => error.Message.Should().Be("Email is already in use"),
            _ => throw new Exception("Expected error, got success")
        );
    }

    [Fact]
    public async Task Should_Return_Success_When_Email_Not_In_Use()
    {
        var command = NewUserCommand();
        SetupValidUserCreation(command);

        var result = await _sut.ExecuteAsync(command, CancellationToken.None);

        var created = result.Match(
            _ => throw new Exception("Expected success, got error"),
            success => success
        );

        created.Email.Should().Be(command.Email);
    }

    [Fact]
    public async Task Should_Hash_Password_When_Creating_User()
    {
        var command = NewUserCommand();
        SetupValidUserCreation(command);

        await _sut.ExecuteAsync(command, CancellationToken.None);

        _passwordHasherMock.Verify(x => x.Hash(command.Password), Times.Once);
    }

    [Fact]
    public async Task Should_Save_User_With_Hashed_Password_And_Free_Plan()
    {
        var command = NewUserCommand();
        SetupValidUserCreation(command);

        await _sut.ExecuteAsync(command, CancellationToken.None);

        _userRepositoryMock.Verify(
            x => x.AddAsync(It.Is<User>(u =>
                u.Email == command.Email &&
                u.Password == "hashedPassword" &&
                u.Plan == Domain.Enums.Plan.Free
            ), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    public async Task Should_Commit_Transaction()
    {
        var command = NewUserCommand();
        SetupValidUserCreation(command);

        await _sut.ExecuteAsync(command, CancellationToken.None);

        _unitOfWorkMock.Verify(
            x => x.CommitAsync(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    private static CreateUserCommand NewUserCommand() =>
        new("test@example.com", "123456");

    private void SetupValidUserCreation(CreateUserCommand command)
    {
        _userRepositoryMock
            .Setup(x => x.ExistByAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _passwordHasherMock
            .Setup(x => x.Hash(command.Password))
            .Returns("hashedPassword");

        _userRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Callback<User, CancellationToken>((user, _) => user.Id = 42)
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
    }
}
