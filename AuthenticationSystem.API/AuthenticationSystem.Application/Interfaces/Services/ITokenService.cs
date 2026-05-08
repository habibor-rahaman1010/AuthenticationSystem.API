using System.Security.Claims;
using AuthenticationSystem.Domain.Entities;

namespace AuthenticationSystem.Application.Interfaces.Services;

public interface ITokenService
{
    string GenerateAccessToken(User user, IEnumerable<string> roles);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
