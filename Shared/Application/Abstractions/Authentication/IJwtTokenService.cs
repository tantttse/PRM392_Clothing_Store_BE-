using System.Security.Claims;

namespace Shared.Application.Abstractions.Authentication;

public interface IJwtTokenService
{
    string GenerateToken(Guid userId, string? email, string? name, IEnumerable<string> roles);
    ClaimsPrincipal? ValidateToken(string token);
    string GenerateRefreshToken();
    bool IsTokenExpired(string token);
}
