using MassLab.Identity.Domain.Entities;

namespace MassLab.Identity.Domain.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken(User user);
    System.Security.Claims.ClaimsPrincipal? ValidateRefreshToken(string token);
}
