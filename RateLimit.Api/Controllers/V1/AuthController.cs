using Microsoft.AspNetCore.Mvc;
using RateLimit.Application.Dtos.Commands.Auth;
using RateLimit.Application.Dtos.Commands.Users;
using RateLimit.Application.Interfaces.UseCases;

namespace RateLimit.Api.Controllers.V1;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthController(
    ICreateUserUseCase createUserUseCase,
    IAuthenticateUserUseCase authenticateUserUseCase
) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] CreateUserCommand command,
        CancellationToken cancellationToken
    )
    {
        var result = await createUserUseCase.ExecuteAsync(command, cancellationToken);

        return result.Match<IActionResult>(
            error => error.Code switch
            {
                "validation" => BadRequest(error),
                "conflict" => Conflict(error),
                "unauthorized" => Unauthorized(error),
                _ => StatusCode(500, error)
            },
            success => CreatedAtAction(nameof(Register), new { id = success.Id }, success)
        );
    }

    [HttpPost("token")]
    public async Task<IActionResult> Login(
        [FromBody] AuthenticateUserCommand command,
        CancellationToken cancellationToken
    )
    {
        var result = await authenticateUserUseCase.ExecuteAsync(command, cancellationToken);

        return result.Match<IActionResult>(
            error => error.Code switch
            {
                "validation" => BadRequest(error),
                "conflict" => Conflict(error),
                "unauthorized" => Unauthorized(error),
                _ => StatusCode(500, error)
            },
            Ok
        );
    }
}