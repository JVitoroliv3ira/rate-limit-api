using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RateLimit.Api.Attributes;

namespace RateLimit.Api.Controllers.V1;

[Authorize]
[Route("api/v1/[controller]")]
[ApiController]
public class LimitedController : ControllerBase
{
    [RateLimit]
    [HttpGet]
    public IActionResult Hello() => Ok("Hello, World!");
}