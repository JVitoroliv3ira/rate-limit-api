using System.ComponentModel.DataAnnotations;

namespace RateLimit.Application.Dtos.Commands.Users;

public record CreateUserCommand(
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is invalid")]
    string Email,

    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    string Password
);
