using MassLab.Registry.Application.Commands.Client.CreateClient;
using MassLab.Registry.Domain.Entities;
using MassLab.Registry.Domain.Interfaces;
using MassLab.Shared.ValueObject;
using Moq;

namespace MassLab.Registry.Application.Tests.Commands;

public class CreateClientHandlerTests
{
    private readonly Mock<IClientRepository> _repositoryMock;
    private readonly CreateClientHandler _handler;

    public CreateClientHandlerTests()
    {
        _repositoryMock = new Mock<IClientRepository>();
        _handler = new CreateClientHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenClientIsValid()
    {
        // Arrange
        var command = new CreateClientCommand(
            "Client Ltda",
            "Client",
            "12345678000195", // CNPJ
            "Rua C", "789", null, "Bairro C", "11111111", "Cidade C", "MG",
            "client@example.com", "11777777777"
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Client Ltda", result.Data.LegalName);
        Assert.Equal("12345678000195", result.Data.DocumentNumber);
        Assert.Equal("client@example.com", result.Data.Email);
        
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WithCpf_WhenDocumentLengthIs11()
    {
        // Arrange
        var command = new CreateClientCommand(
            "Client PF",
            null,
            "12345678909", // CPF
            null, null, null, null, null, null, null,
            null, null
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data.DocumentNumber);
        Assert.Equal("123.456.789-09", Cpf.Create(result.Data.DocumentNumber!).Data.Formatted);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenDocumentIsInvalid()
    {
        // Arrange
        var command = new CreateClientCommand(
            "Client Invalid",
            null,
            "123", // Invalid length
            null, null, null, null, null, null, null,
            null, null
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
