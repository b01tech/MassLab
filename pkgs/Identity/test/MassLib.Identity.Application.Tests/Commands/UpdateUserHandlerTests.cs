using MassLib.Identity.Application.Commands.UpdateUser;
using MassLib.Identity.Domain.Entities;
using MassLib.Identity.Domain.Enums;
using MassLib.Identity.Domain.Interfaces;
using MassLib.Shared.Errors;
using MassLib.Shared.Persistence;
using Moq;

namespace MassLib.Identity.Application.Tests.Commands;

public class UpdateUserHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly UpdateUserHandler _handler;

    public UpdateUserHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _uowMock = new Mock<IUnitOfWork>();
        _handler = new UpdateUserHandler(_userRepositoryMock.Object, _uowMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUserNotFound()
    {
        // Arrange
        var command = new UpdateUserCommand(Guid.CreateVersion7(), "new_name", UserRole.Manager);

        _userRepositoryMock.Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessages.USER_NOT_FOUND, result.Errors);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenUserUpdatedSuccessfully()
    {
        // Arrange
        var user = User.Create("old_name", "hash", UserRole.Operator).Data;
        var command = new UpdateUserCommand(user.Id, "new_name", UserRole.Manager);

        _userRepositoryMock.Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(command.UserName, result.Data.UserName);
        Assert.Equal(command.Role, result.Data.Role);

        _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        _uowMock.Verify(x => x.CommitAsync(), Times.Once);
    }
}
