using MassLib.Identity.Domain.Entities;

namespace MassLib.Identity.Domain.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
}
