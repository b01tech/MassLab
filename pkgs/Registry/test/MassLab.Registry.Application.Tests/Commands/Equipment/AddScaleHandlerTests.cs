using MassLab.Registry.Application.Commands.Equipment.AddScale;
using MassLab.Registry.Domain.Entities;
using MassLab.Registry.Domain.Enums;
using MassLab.Registry.Domain.Interfaces;
using MassLab.Shared.Persistence;
using MassLab.Shared.ValueObject;
using Moq;

namespace MassLab.Registry.Application.Tests.Commands.Equipment;

public class AddScaleHandlerTests
{
    private readonly Mock<IClientRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly AddScaleHandler _handler;

    public AddScaleHandlerTests()
    {
        _repositoryMock = new Mock<IClientRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new AddScaleHandler(_repositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenClientExistsAndScaleIsValid()
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

        var command = new AddScaleCommand(
            clientId,
            "TAG-001",
            "Manufacturer X",
            "Model Y",
            "ID-123",
            100.0f,
            0.1f,
            "SN-999",
            ScaleClass.CLASS_III
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("TAG-001", result.Data.Tag);
        Assert.Equal("SN-999", result.Data.SerialNumber);
        
        _repositoryMock.Verify(x => x.UpdateAsync(client, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenClientNotFound()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        _repositoryMock
            .Setup(x => x.GetByIdWithEquipmentsAsync(clientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Client?)null);

        var command = new AddScaleCommand(
            clientId,
            "TAG-001",
            "Manufacturer X",
            "Model Y",
            "ID-123",
            100.0f,
            0.1f,
            "SN-999",
            ScaleClass.CLASS_III
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Client not found", result.Errors.First());
        
        _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
