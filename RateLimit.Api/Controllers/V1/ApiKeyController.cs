using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RateLimit.Application.Dtos.Commands.ApiKeys;
using RateLimit.Application.Interfaces.Services;
using RateLimit.Application.Interfaces.UseCases;

namespace RateLimit.Api.Controllers.V1;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class ApiKeyController(
    IAuthenticatedUserService authenticatedUserService,
    ICreateApiKeyUseCase createApiKeyUseCase,
    IDeleteApiKeyUseCase deleteApiKeyUseCase
) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateApiKeyCommand command,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = authenticatedUserService.UserId;
        if (userId is null)
        {
            return Unauthorized();
        }

        var result = await createApiKeyUseCase.ExecuteAsync(command, userId.Value, cancellationToken);

        return result.Match<IActionResult>(
            error => error.Code switch
            {
                "validation" => BadRequest(error),
                "conflict" => Conflict(error),
                "unauthorized" => Unauthorized(error),
                _ => StatusCode(500, error)
            },
            success => CreatedAtAction(
                nameof(Create),
                new { id = success.Name },
                success
            )
        );
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        [FromRoute] int id,
        CancellationToken cancellationToken
    )
    {
        var userId = authenticatedUserService.UserId;
        if (userId is null)
        {
            return Unauthorized();
        }

        var result = await deleteApiKeyUseCase.ExecuteAsync(id, userId.Value, cancellationToken);

        return result.Match<IActionResult>(
            error => error.Code switch
            {
                "validation" => BadRequest(error),
                "unauthorized" => Unauthorized(error),
                "not_found" => NotFound(error),
                _ => StatusCode(500, error)
            },
            deleted => deleted ? NoContent() : NotFound()
        );
    }
}
