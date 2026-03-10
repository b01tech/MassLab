using MassLab.Registry.Application.Commands.Client.UpdateClient;
using MassLab.Registry.Domain.Entities;
using MassLab.Registry.Domain.Interfaces;
using MassLab.Shared.ValueObject;
using Moq;

namespace MassLab.Registry.Application.Tests.Commands;

public class UpdateClientHandlerTests
{
    private readonly Mock<IClientRepository> _repositoryMock;
    private readonly UpdateClientHandler _handler;

    public UpdateClientHandlerTests()
    {
        _repositoryMock = new Mock<IClientRepository>();
        _handler = new UpdateClientHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenClientExists()
    {
        // Arrange
        var existingClient = Client.Create(
            Name.Create("Old Name").Data,
            Name.Create("Old Business").Data,
            Cnpj.Create("12345678000195").Data
        ).Data;

        _repositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingClient);

        var command = new UpdateClientCommand(
            existingClient.Id,
            "New Name",
            "New Business",
            "Rua D", "101", null, "Bairro D", "22222222", "Cidade D", "RS",
            "update@example.com", "11666666666"
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("New Name", result.Data.LegalName);
        Assert.Equal("update@example.com", result.Data.Email);

        _repositoryMock.Verify(
            x => x.UpdateAsync(
                It.Is<Client>(c => c.LegalName.Value == "New Name"),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenClientNotFound()
    {
        // Arrange
        _repositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Client?)null);

        var command = new UpdateClientCommand(
            Guid.NewGuid(), "New Name", null, null, null, null, null, null, null, null, null, null
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("Client not found", result.Errors);
    }
}
