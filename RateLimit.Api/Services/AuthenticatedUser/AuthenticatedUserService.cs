using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using RateLimit.Application.Interfaces.Services;

namespace RateLimit.Api.Services.AuthenticatedUser;

public class AuthenticatedUserService(
    IHttpContextAccessor httpContextAccessor
) : IAuthenticatedUserService
{
    public int? UserId
    {
        get
        {
            var user = httpContextAccessor.HttpContext?.User;
            var idClaim = user?.FindFirst(JwtRegisteredClaimNames.Sub) 
                          ?? user?.FindFirst(ClaimTypes.NameIdentifier);

            return int.TryParse(idClaim?.Value, out var id) ? id : null;
        }
    }
}