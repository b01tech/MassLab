using MassLib.Identity.Application.Commands.Login;
using MassLib.Identity.Domain.Entities;
using MassLib.Identity.Domain.Enums;
using MassLib.Identity.Domain.Interfaces;
using MassLib.Shared.Errors;
using Moq;

namespace MassLib.Identity.Application.Tests.Commands;

public class LoginHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IEncrypter> _encrypterMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly LoginHandler _handler;

    public LoginHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _encrypterMock = new Mock<IEncrypter>();
        _tokenServiceMock = new Mock<ITokenService>();
        _handler = new LoginHandler(_userRepositoryMock.Object, _encrypterMock.Object, _tokenServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUserNotFound()
    {
        // Arrange
        var command = new LoginCommand("non_existent", "password");

        _userRepositoryMock
            .Setup(x => x.GetByUserNameAsync(command.UserName, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessages.CREDENTIALS_INVALID, result.Errors);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenPasswordIsInvalid()
    {
        // Arrange
        var command = new LoginCommand("user", "wrong_password");
        var user = User.Create("user", "hash", "Operator").Data;

        _userRepositoryMock
            .Setup(x => x.GetByUserNameAsync(command.UserName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _encrypterMock.Setup(x => x.Verify(command.Password, user.HashPassword.Value)).Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessages.CREDENTIALS_INVALID, result.Errors);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUserIsInactive()
    {
        // Arrange
        var command = new LoginCommand("inactive_user", "password");
        var user = User.Create("inactive_user", "hash", "Operator").Data;
        user.Deactivate();

        _userRepositoryMock
            .Setup(x => x.GetByUserNameAsync(command.UserName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _encrypterMock.Setup(x => x.Verify(command.Password, user.HashPassword.Value)).Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessages.USER_INACTIVE, result.Errors);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenCredentialsAreValid()
    {
        // Arrange
        var command = new LoginCommand("valid_user", "password");
        var user = User.Create("valid_user", "hash", "Operator").Data;

        _userRepositoryMock
            .Setup(x => x.GetByUserNameAsync(command.UserName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _encrypterMock.Setup(x => x.Verify(command.Password, user.HashPassword.Value)).Returns(true);

        _tokenServiceMock.Setup(x => x.GenerateAccessToken(user)).Returns("access_token");
        _tokenServiceMock.Setup(x => x.GenerateRefreshToken()).Returns("refresh_token");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("access_token", result.Data.AccessToken);
        Assert.Equal("refresh_token", result.Data.RefreshToken);
    }
}
