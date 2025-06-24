namespace RateLimit.Application.Dtos.Results.Auth;

public record AuthenticationResult(
    string AccessToken,
    int UserId
);