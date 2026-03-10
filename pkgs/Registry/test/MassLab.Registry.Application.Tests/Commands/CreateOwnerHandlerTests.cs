using MassLab.Registry.Application.Commands.Owner.CreateOwner;
using MassLab.Registry.Domain.Entities;
using MassLab.Registry.Domain.Interfaces;
using MassLab.Shared.ValueObject;
using Moq;

namespace MassLab.Registry.Application.Tests.Commands;

public class CreateOwnerHandlerTests
{
    private readonly Mock<IOwnerRepository> _repositoryMock;
    private readonly CreateOwnerHandler _handler;

    public CreateOwnerHandlerTests()
    {
        _repositoryMock = new Mock<IOwnerRepository>();
        _handler = new CreateOwnerHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenOwnerIsValid()
    {
        // Arrange
        var command = new CreateOwnerCommand(
            "MassLab Ltda",
            "MassLab",
            "12345678000195",
            "Rua A", "123", null, "Bairro", "12345678", "Cidade", "SP",
            "test@masslab.com", "11999999999", "http://logo.url"
        );

        _repositoryMock
            .Setup(x => x.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((Owner?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("MassLab Ltda", result.Data.LegalName);
        Assert.Equal("MassLab", result.Data.BusinessName);
        Assert.Equal("12.345.678/0001-95", result.Data.Cnpj);
        Assert.Equal("test@masslab.com", result.Data.Email);
        
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<Owner>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenOwnerAlreadyExists()
    {
        // Arrange
        var command = new CreateOwnerCommand(
            "MassLab Ltda",
            "MassLab",
            "12345678000195",
            null, null, null, null, null, null, null,
            null, null, null
        );

        var existingOwner = Owner.Create(
            Name.Create("Existing").Data,
            Name.Create("Existing").Data,
            Cnpj.Create("12345678000195").Data
        ).Data;

        _repositoryMock
            .Setup(x => x.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingOwner);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("Owner already exists", result.Errors);
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<Owner>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenValidationFails()
    {
        // Arrange
        var command = new CreateOwnerCommand(
            "", // Invalid Name
            "MassLab",
            "12345678000195",
            null, null, null, null, null, null, null,
            null, null, null
        );

        _repositoryMock
            .Setup(x => x.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((Owner?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<Owner>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
