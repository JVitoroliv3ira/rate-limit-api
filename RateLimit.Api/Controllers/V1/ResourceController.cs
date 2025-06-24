using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RateLimit.Api.Controllers.V1;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class ResourceController : ControllerBase
{
    [HttpGet]
    public IActionResult Hello() => Ok("Hello");
}