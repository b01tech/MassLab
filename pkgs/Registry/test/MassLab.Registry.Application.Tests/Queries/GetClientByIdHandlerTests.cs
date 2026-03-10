using MassLab.Registry.Application.Queries.Client.GetClientById;
using MassLab.Registry.Domain.Entities;
using MassLab.Registry.Domain.Interfaces;
using MassLab.Shared.ValueObject;
using Moq;

namespace MassLab.Registry.Application.Tests.Queries;

public class GetClientByIdHandlerTests
{
    private readonly Mock<IClientRepository> _repositoryMock;
    private readonly GetClientByIdHandler _handler;

    public GetClientByIdHandlerTests()
    {
        _repositoryMock = new Mock<IClientRepository>();
        _handler = new GetClientByIdHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnClient_WhenExists()
    {
        // Arrange
        var existingClient = Client.Create(
            Name.Create("Client").Data,
            null,
            Cnpj.Create("12345678000195").Data
        ).Data;

        _repositoryMock
            .Setup(x => x.GetByIdAsync(existingClient.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingClient);

        var query = new GetClientByIdQuery(existingClient.Id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(existingClient.Id, result.Data.Id);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenClientNotFound()
    {
        // Arrange
        _repositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Client?)null);

        var query = new GetClientByIdQuery(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("Client not found", result.Errors);
    }
}
