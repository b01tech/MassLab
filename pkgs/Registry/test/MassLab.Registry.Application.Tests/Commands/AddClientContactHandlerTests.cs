using MassLab.Registry.Application.Commands.Client.AddClientContact;
using MassLab.Registry.Domain.Entities;
using MassLab.Registry.Domain.Interfaces;
using MassLab.Shared.ValueObject;
using Moq;

namespace MassLab.Registry.Application.Tests.Commands;

public class AddClientContactHandlerTests
{
    private readonly Mock<IClientRepository> _repositoryMock;
    private readonly AddClientContactHandler _handler;

    public AddClientContactHandlerTests()
    {
        _repositoryMock = new Mock<IClientRepository>();
        _handler = new AddClientContactHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenClientExists()
    {
        // Arrange
        var existingClient = Client.Create(
            Name.Create("Client").Data,
            null,
            Cnpj.Create("12345678000195").Data
        ).Data;

        _repositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingClient);

        var command = new AddClientContactCommand(
            existingClient.Id,
            "John Doe",
            "john@example.com",
            "11999999999"
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Data.Contacts);
        Assert.Equal("John Doe", result.Data.Contacts[0].Name);

        _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenContactInvalid()
    {
        // Arrange
        var existingClient = Client.Create(
            Name.Create("Client").Data,
            null,
            Cnpj.Create("12345678000195").Data
        ).Data;

        _repositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingClient);

        var command = new AddClientContactCommand(
            existingClient.Id,
            "", // Invalid Name
            "john@example.com",
            "11999999999"
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
