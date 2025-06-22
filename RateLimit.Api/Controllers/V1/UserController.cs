using Microsoft.AspNetCore.Mvc;
using RateLimit.Application.Dtos.Commands.Users;
using RateLimit.Application.Interfaces.UseCases;

namespace RateLimit.Api.Controllers.V1;

[Route("api/v1/[controller]")]
[ApiController]
public class UserController(
    ICreateUserUseCase createUserUseCase    
) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
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
            success => CreatedAtAction(nameof(Create), new { id = success.Id }, success)
        );
    }
}