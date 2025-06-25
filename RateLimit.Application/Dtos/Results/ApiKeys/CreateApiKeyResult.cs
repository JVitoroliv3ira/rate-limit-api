namespace RateLimit.Application.Dtos.Results.ApiKeys;

public record CreateApiKeyResult(
    string Name,
    string Key
);