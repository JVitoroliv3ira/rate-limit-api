namespace RateLimit.Application.Dtos.Commands.Auth;

public record AuthenticateUserCommand(string Email, string Password);
