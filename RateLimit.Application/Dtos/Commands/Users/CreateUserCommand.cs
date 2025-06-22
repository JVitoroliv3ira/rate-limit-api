namespace RateLimit.Application.Dtos.Commands.Users;

public record CreateUserCommand(
    string Email,
    string Password
);
