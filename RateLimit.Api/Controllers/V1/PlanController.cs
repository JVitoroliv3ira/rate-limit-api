using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RateLimit.Application.Interfaces.UseCases;
using RateLimit.Application.Interfaces.Services;
using RateLimit.Domain.Enums;

namespace RateLimit.Api.Controllers.V1;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class PlanController(
    IAuthenticatedUserService authenticatedUserService,
    IChangePlanUseCase changePlanUseCase
) : ControllerBase
{
    [HttpPut]
    public async Task<IActionResult> Change(
        [FromBody] Plan plan,
        CancellationToken cancellationToken
    )
    {
        var userId = authenticatedUserService.UserId;
        if (userId is null)
        {
            return Unauthorized();
        }

        var result = await changePlanUseCase.ExecuteAsync(plan, userId.Value, cancellationToken);

        return result.Match<IActionResult>(
            error => error.Code switch
            {
                "validation" => BadRequest(error),
                "unauthorized" => Unauthorized(error),
                "not_found" => NotFound(error),
                _ => StatusCode(500, error)
            },
            _ => NoContent()
        );
    }
}