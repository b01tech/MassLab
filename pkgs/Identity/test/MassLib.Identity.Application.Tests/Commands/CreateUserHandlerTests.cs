using MassLib.Identity.Application.Commands.CreateUser;
using MassLib.Identity.Domain.Entities;
using MassLib.Identity.Domain.Enums;
using MassLib.Identity.Domain.Interfaces;
using MassLib.Shared.Persistence;
using Moq;

namespace MassLib.Identity.Application.Tests.Commands;

public class CreateUserHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<IEncrypter> _encrypterMock;
    private readonly CreateUserHandler _handler;

    public CreateUserHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _uowMock = new Mock<IUnitOfWork>();
        _encrypterMock = new Mock<IEncrypter>();
        _handler = new CreateUserHandler(_userRepositoryMock.Object, _uowMock.Object, _encrypterMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUserAlreadyExists()
    {
        // Arrange
        var command = new CreateUserCommand("existing_user", "ValidPass123!", UserRole.Operator);
        
        // Mock existing user
        var existingUser = User.Create("existing_user", "hash", UserRole.Operator).Data;
        _userRepositoryMock.Setup(x => x.GetByUserNameAsync(command.UserName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUserCreationFails()
    {
        // Arrange - Invalid username to trigger domain validation failure
        var command = new CreateUserCommand("", "ValidPass123!", UserRole.Operator); 
        
        _userRepositoryMock.Setup(x => x.GetByUserNameAsync(command.UserName, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);
            
        _encrypterMock.Setup(x => x.Encrypt(command.Password)).Returns("hashed_password");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenUserCreatedSuccessfully()
    {
        // Arrange
        var command = new CreateUserCommand("new_user", "ValidPass123!", UserRole.Admin);
        
        _userRepositoryMock.Setup(x => x.GetByUserNameAsync(command.UserName, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);
            
        _encrypterMock.Setup(x => x.Encrypt(command.Password)).Returns("hashed_password");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(command.UserName, result.Data.UserName);
        Assert.Equal(command.Role, result.Data.Role);
        Assert.True(result.Data.Active);
        
        _userRepositoryMock.Verify(x => x.AddAsync(It.Is<User>(u => 
            u.UserName.Value == command.UserName && 
            u.HashPassword.Value == "hashed_password" &&
            u.Role == command.Role), It.IsAny<CancellationToken>()), Times.Once);
            
        _uowMock.Verify(x => x.CommitAsync(), Times.Once);
    }
}

