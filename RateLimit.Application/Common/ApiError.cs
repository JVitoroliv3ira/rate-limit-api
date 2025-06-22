namespace RateLimit.Application.Common;

public class ApiError
{
    public string Code { get; }
    public string Message { get; }

    public ApiError(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public static ApiError NotFound(string message) => new("not_found", message);
    public static ApiError Conflict(string message) => new("conflict", message);
    public static ApiError Validation(string message) => new("validation", message);
    public static ApiError Unauthorized(string message) => new("unauthorized", message);
    public static ApiError Internal(string message) => new("internal", message);
}