using MassLab.Identity.Application.Commands.ActivateUser;
using MassLab.Identity.Domain.Entities;
using MassLab.Identity.Domain.Enums;
using MassLab.Identity.Domain.Interfaces;
using MassLab.Shared.Errors;
using MassLab.Shared.Persistence;
using Moq;

namespace MassLab.Identity.Application.Tests.Commands;

public class ActivateUserHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly ActivateUserHandler _handler;

    public ActivateUserHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _uowMock = new Mock<IUnitOfWork>();
        _handler = new ActivateUserHandler(_userRepositoryMock.Object, _uowMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUserNotFound()
    {
        // Arrange
        var command = new ActivateUserCommand(Guid.CreateVersion7());

        _userRepositoryMock.Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessages.USER_NOT_FOUND, result.Errors);
    }

    [Fact]
    public async Task Handle_ShouldActivateUser_WhenUserExists()
    {
        // Arrange
        var user = User.Create("user", "hash", "Operator").Data;
        user.Deactivate(); // Ensure it starts deactivated
        var command = new ActivateUserCommand(user.Id);

        _userRepositoryMock.Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(user.Active);

        _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        _uowMock.Verify(x => x.CommitAsync(), Times.Once);
    }
}
