using MassLab.Identity.Application.DTOs;
using MassLab.Identity.Domain.Interfaces;
using MassLab.Shared.Errors;
using MassLab.Shared.Results;

using MassLab.Shared.Persistence;

namespace MassLab.Identity.Application.Commands.Login;

public class LoginHandler(IUserRepository userRepository, IEncrypter encrypter, ITokenService tokenService)
{

    public async Task<Result<TokenResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByUserNameAsync(request.UserName, cancellationToken);
        if (user is null)
        {
            return Result<TokenResponse>.Failure(ErrorMessages.CREDENTIALS_INVALID);
        }

        if (!encrypter.Verify(request.Password, user.HashPassword.Value))
        {
            return Result<TokenResponse>.Failure(ErrorMessages.CREDENTIALS_INVALID);
        }

        if (!user.Active)
        {
            return Result<TokenResponse>.Failure(ErrorMessages.USER_INACTIVE);
        }

        var accessToken = tokenService.GenerateAccessToken(user);
        var refreshToken = tokenService.GenerateRefreshToken(user);

        return new TokenResponse(accessToken, refreshToken);
    }
}

