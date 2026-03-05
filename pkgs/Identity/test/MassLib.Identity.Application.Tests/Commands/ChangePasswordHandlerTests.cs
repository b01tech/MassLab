using MassLib.Identity.Application.Commands.ChangePassword;
using MassLib.Identity.Domain.Entities;
using MassLib.Identity.Domain.Enums;
using MassLib.Identity.Domain.Interfaces;
using MassLib.Shared.Errors;
using MassLib.Shared.Persistence;
using Moq;

namespace MassLib.Identity.Application.Tests.Commands;

public class ChangePasswordHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<IEncrypter> _encrypterMock;
    private readonly ChangePasswordHandler _handler;

    public ChangePasswordHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _uowMock = new Mock<IUnitOfWork>();
        _encrypterMock = new Mock<IEncrypter>();
        _handler = new ChangePasswordHandler(_userRepositoryMock.Object, _uowMock.Object, _encrypterMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUserNotFound()
    {
        // Arrange
        var command = new ChangePasswordCommand(Guid.CreateVersion7(), "NewPass123!");

        _userRepositoryMock.Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessages.USER_NOT_FOUND, result.Errors);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenPasswordChangedSuccessfully()
    {
        // Arrange
        var user = User.Create("user", "old_hash", UserRole.Operator).Data;
        var command = new ChangePasswordCommand(user.Id, "NewPass123!");
        var newHash = "new_hashed_password";

        _userRepositoryMock.Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _encrypterMock.Setup(x => x.Encrypt(command.NewPassword)).Returns(newHash);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(newHash, user.HashPassword.Value);

        _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        _uowMock.Verify(x => x.CommitAsync(), Times.Once);
    }
}
