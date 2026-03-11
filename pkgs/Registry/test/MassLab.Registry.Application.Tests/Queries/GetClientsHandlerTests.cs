using MassLab.Registry.Application.Queries.Client.GetClients;
using MassLab.Registry.Domain.Entities;
using MassLab.Registry.Domain.Interfaces;
using MassLab.Shared.ValueObject;
using Moq;

namespace MassLab.Registry.Application.Tests.Queries;

public class GetClientsHandlerTests
{
    private readonly Mock<IClientRepository> _repositoryMock;
    private readonly GetClientsHandler _handler;

    public GetClientsHandlerTests()
    {
        _repositoryMock = new Mock<IClientRepository>();
        _handler = new GetClientsHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnPaginatedClients()
    {
        // Arrange
        var clients = new List<Client>
        {
            Client.Create(Name.Create("Client A").Data, null, Cnpj.Create("12345678000195").Data).Data,
            Client.Create(Name.Create("Client B").Data, null, Cnpj.Create("98765432000195").Data).Data
        };

        _repositoryMock
            .Setup(x => x.GetAllAsync(1, 10, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(clients);

        _repositoryMock
            .Setup(x => x.GetTotalCountAsync(null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(2);

        var query = new GetClientsQuery(1, 10);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Data.Pagination.TotalItems);
        Assert.Equal(2, result.Data.Items.Count());
        Assert.Contains(result.Data.Items, c => c.LegalName == "Client A");
        Assert.Contains(result.Data.Items, c => c.LegalName == "Client B");
    }

    [Fact]
    public async Task Handle_ShouldReturnEmpty_WhenNoClients()
    {
        // Arrange
        _repositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Client>());

        _repositoryMock
            .Setup(x => x.GetTotalCountAsync(It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        var query = new GetClientsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Data.Items);
        Assert.Equal(0, result.Data.Pagination.TotalItems);
    }
}
