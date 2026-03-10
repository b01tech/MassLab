using MassLab.Registry.Application.Commands.Client.RemoveClientContact;
using MassLab.Registry.Domain.Entities;
using MassLab.Registry.Domain.Interfaces;
using MassLab.Shared.ValueObject;
using Moq;

namespace MassLab.Registry.Application.Tests.Commands;

public class RemoveClientContactHandlerTests
{
    private readonly Mock<IClientRepository> _repositoryMock;
    private readonly RemoveClientContactHandler _handler;

    public RemoveClientContactHandlerTests()
    {
        _repositoryMock = new Mock<IClientRepository>();
        _handler = new RemoveClientContactHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldRemoveContact_WhenClientAndContactExist()
    {
        // Arrange
        var existingClient = Client.Create(
            Name.Create("Client").Data,
            null,
            Cnpj.Create("12345678000195").Data
        ).Data;

        var contact = Contact.Create("John Doe", "john@example.com", "11999999999").Data;
        existingClient.AddContact(contact);

        _repositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingClient);

        var command = new RemoveClientContactCommand(
            existingClient.Id,
            "john@example.com",
            "11999999999"
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Data.Contacts);

        _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
