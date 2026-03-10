using MassLab.Registry.Application.Commands.Owner.UpdateOwner;
using MassLab.Registry.Domain.Entities;
using MassLab.Registry.Domain.Interfaces;
using MassLab.Shared.ValueObject;
using Moq;

namespace MassLab.Registry.Application.Tests.Commands;

public class UpdateOwnerHandlerTests
{
    private readonly Mock<IOwnerRepository> _repositoryMock;
    private readonly UpdateOwnerHandler _handler;

    public UpdateOwnerHandlerTests()
    {
        _repositoryMock = new Mock<IOwnerRepository>();
        _handler = new UpdateOwnerHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenOwnerExists()
    {
        // Arrange
        var existingOwner = Owner.Create(
            Name.Create("Old Name").Data,
            Name.Create("Old Business").Data,
            Cnpj.Create("12345678000195").Data
        ).Data;

        _repositoryMock
            .Setup(x => x.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingOwner);

        var command = new UpdateOwnerCommand(
            "New Name",
            "New Business",
            "Rua B", "456", null, "Bairro B", "87654321", "Cidade B", "RJ",
            "new@masslab.com", "11888888888", "http://newlogo.url"
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("New Name", result.Data.LegalName);
        Assert.Equal("New Business", result.Data.BusinessName);
        Assert.Equal("new@masslab.com", result.Data.Email);

        _repositoryMock.Verify(
            x => x.UpdateAsync(
                It.Is<Owner>(o => o.LegalName.Value == "New Name" && o.Email.Value == "new@masslab.com"),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenOwnerNotFound()
    {
        // Arrange
        _repositoryMock
            .Setup(x => x.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((Owner?)null);

        var command = new UpdateOwnerCommand(
            "New Name", "New Business", null, null, null, null, null, null, null, null, null, null
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("Owner not found", result.Errors);
        _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Owner>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
