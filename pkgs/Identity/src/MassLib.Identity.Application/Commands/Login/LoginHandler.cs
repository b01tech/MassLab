using MassLib.Identity.Application.DTOs;
using MassLib.Identity.Domain.Interfaces;
using MassLib.Shared.Results;

namespace MassLib.Identity.Application.Commands.Login;

public class LoginHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IEncrypter _encrypter;
    private readonly ITokenService _tokenService;

    public LoginHandler(IUserRepository userRepository, IEncrypter encrypter, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _encrypter = encrypter;
        _tokenService = tokenService;
    }

    public async Task<Result<TokenResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUserNameAsync(request.UserName, cancellationToken);
        if (user is null)
        {
            return Result<TokenResponse>.Failure("Invalid credentials.");
        }

        if (!_encrypter.Verify(request.Password, user.HashPassword.Value))
        {
            return Result<TokenResponse>.Failure("Invalid credentials.");
        }

        if (!user.Active)
        {
            return Result<TokenResponse>.Failure("User is inactive.");
        }

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        return new TokenResponse(accessToken, refreshToken);
    }
}

