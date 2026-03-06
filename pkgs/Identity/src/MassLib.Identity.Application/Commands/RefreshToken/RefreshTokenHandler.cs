using MassLib.Identity.Application.DTOs;
using MassLib.Identity.Domain.Interfaces;
using MassLib.Shared.Errors;
using MassLib.Shared.Results;
using System.Security.Claims;

namespace MassLib.Identity.Application.Commands.RefreshToken;

public class RefreshTokenHandler(IUserRepository userRepository, ITokenService tokenService)
{
    public async Task<Result<TokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var principal = tokenService.ValidateRefreshToken(request.RefreshToken);
        if (principal is null)
        {
            return Result<TokenResponse>.Failure(ErrorMessages.REFRESH_TOKEN_INVALID);
        }

        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return Result<TokenResponse>.Failure(ErrorMessages.REFRESH_TOKEN_INVALID);


        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null || !user.Active)
            return Result<TokenResponse>.Failure(ErrorMessages.USER_INACTIVE);


        var newAccessToken = tokenService.GenerateAccessToken(user);
        var newRefreshToken = tokenService.GenerateRefreshToken(user);

        return new TokenResponse(newAccessToken, newRefreshToken);
    }
}
