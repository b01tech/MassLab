using MassLab.Registry.Application.DTOs;
using MassLab.Registry.Application.Queries.Equipment.GetClientEquipments;
using MassLab.Registry.Domain.Entities;
using MassLab.Registry.Domain.Enums;
using MassLab.Registry.Domain.Interfaces;
using MassLab.Shared.ValueObject;
using Moq;

namespace MassLab.Registry.Application.Tests.Queries.Equipment;

public class GetClientEquipmentsHandlerTests
{
    private readonly Mock<IClientRepository> _repositoryMock;
    private readonly GetClientEquipmentsHandler _handler;

    public GetClientEquipmentsHandlerTests()
    {
        _repositoryMock = new Mock<IClientRepository>();
        _handler = new GetClientEquipmentsHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnEquipments_WhenClientExists()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var client = Client.Create(
            Name.Create("Client Ltda").Data,
            Name.Create("Client").Data,
            Cnpj.Create("12345678000195").Data
        ).Data;

        var scale = Scale.Create(
            "TAG-001",
            "Manufacturer X",
            "Model Y",
            "ID-123",
            100.0f,
            0.1f,
            "SN-999",
            ScaleClass.CLASS_III
        ).Data;

        client.AddEquipment(scale);

        _repositoryMock
            .Setup(x => x.GetByIdWithEquipmentsAsync(clientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(client);

        var query = new GetClientEquipmentsQuery(clientId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Data);
        var equipment = result.Data.First();
        Assert.Equal("TAG-001", equipment.Tag);
        Assert.Equal(EquipmentType.Scale, equipment.Type);
        Assert.IsType<ScaleResponse>(equipment);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenClientNotFound()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        _repositoryMock
            .Setup(x => x.GetByIdWithEquipmentsAsync(clientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Client?)null);

        var query = new GetClientEquipmentsQuery(clientId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Client not found", result.Errors.First());
    }
}
