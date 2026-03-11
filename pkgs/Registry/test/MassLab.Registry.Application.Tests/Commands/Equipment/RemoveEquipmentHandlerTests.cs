using MassLab.Registry.Application.Commands.Equipment.RemoveEquipment;
using MassLab.Registry.Domain.Entities;
using MassLab.Registry.Domain.Enums;
using MassLab.Registry.Domain.Interfaces;
using MassLab.Shared.Persistence;
using MassLab.Shared.ValueObject;
using Moq;

namespace MassLab.Registry.Application.Tests.Commands.Equipment;

public class RemoveEquipmentHandlerTests
{
    private readonly Mock<IClientRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly RemoveEquipmentHandler _handler;

    public RemoveEquipmentHandlerTests()
    {
        _repositoryMock = new Mock<IClientRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new RemoveEquipmentHandler(_repositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenClientAndEquipmentExist()
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

        var command = new RemoveEquipmentCommand(clientId, scale.Id);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        
        _repositoryMock.Verify(x => x.UpdateAsync(client, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenClientNotFound()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var equipmentId = Guid.NewGuid();

        _repositoryMock
            .Setup(x => x.GetByIdWithEquipmentsAsync(clientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Client?)null);

        var command = new RemoveEquipmentCommand(clientId, equipmentId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Client not found", result.Errors.First());
        
        _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenEquipmentNotFound()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var client = Client.Create(
            Name.Create("Client Ltda").Data,
            Name.Create("Client").Data,
            Cnpj.Create("12345678000195").Data
        ).Data;

        _repositoryMock
            .Setup(x => x.GetByIdWithEquipmentsAsync(clientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(client);

        var command = new RemoveEquipmentCommand(clientId, Guid.NewGuid()); // Random ID

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Equipment not found", result.Errors.First());
        
        _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
