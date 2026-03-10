using MassLab.Registry.Application.Queries.Owner.GetOwner;
using MassLab.Registry.Domain.Entities;
using MassLab.Registry.Domain.Interfaces;
using MassLab.Shared.ValueObject;
using Moq;

namespace MassLab.Registry.Application.Tests.Queries;

public class GetOwnerHandlerTests
{
    private readonly Mock<IOwnerRepository> _repositoryMock;
    private readonly GetOwnerHandler _handler;

    public GetOwnerHandlerTests()
    {
        _repositoryMock = new Mock<IOwnerRepository>();
        _handler = new GetOwnerHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnOwner_WhenExists()
    {
        // Arrange
        var existingOwner = Owner.Create(
            Name.Create("MassLab Ltda").Data,
            Name.Create("MassLab").Data,
            Cnpj.Create("12345678000195").Data
        ).Data;

        _repositoryMock
            .Setup(x => x.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingOwner);

        var query = new GetOwnerQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("MassLab Ltda", result.Data.LegalName);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenOwnerNotFound()
    {
        // Arrange
        _repositoryMock
            .Setup(x => x.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((Owner?)null);

        var query = new GetOwnerQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("Owner not found", result.Errors);
    }
}
