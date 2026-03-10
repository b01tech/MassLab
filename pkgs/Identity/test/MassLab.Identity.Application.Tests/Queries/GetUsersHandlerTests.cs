using MassLab.Identity.Application.Queries.GetUsers;
using MassLab.Identity.Domain.Entities;
using MassLab.Identity.Domain.Enums;
using MassLab.Identity.Domain.Interfaces;
using Moq;

namespace MassLab.Identity.Application.Tests.Queries;

public class GetUsersHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly GetUsersHandler _handler;

    public GetUsersHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _handler = new GetUsersHandler(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnPaginatedUsers()
    {
        // Arrange
        var users = new List<User>
        {
            User.Create("user1", "hash1", "Operator").Data!,
            User.Create("user2", "hash2", "Admin").Data!
        };

        _userRepositoryMock
            .Setup(x =>
                x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(users);

        _userRepositoryMock
            .Setup(x => x.GetTotalCountAsync(It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(2);

        var query = new GetUsersQuery(1, 10);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Data.Data.Count());
        Assert.Equal(1, result.Data.Pagination.Page);
        Assert.Equal(10, result.Data.Pagination.PageSize);
        Assert.Equal(2, result.Data.Pagination.TotalItems);
        Assert.Contains(result.Data.Data, u => u.UserName == "user1");
        Assert.Contains(result.Data.Data, u => u.UserName == "user2");
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoUsersExist()
    {
        // Arrange
        _userRepositoryMock
            .Setup(x =>
                x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(new List<User>());

        _userRepositoryMock
            .Setup(x => x.GetTotalCountAsync(It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        var query = new GetUsersQuery(1, 10);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Data.Data);
        Assert.Equal(0, result.Data.Pagination.TotalItems);
    }

    [Fact]
    public async Task Handle_ShouldCapPageSize_WhenExceedsMax()
    {
        // Arrange
        var users = new List<User>();
        _userRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<int>(), 200, It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        _userRepositoryMock
            .Setup(x => x.GetTotalCountAsync(It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        var query = new GetUsersQuery(1, 1000); // PageSize > MaxPageSize

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(200, result.Data.Pagination.PageSize);
        _userRepositoryMock.Verify(
            x => x.GetAllAsync(1, 200, It.IsAny<string?>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnFirstPage_WhenPageIsLessThanOne()
    {
        // Arrange
        var users = new List<User>();
        _userRepositoryMock
            .Setup(x => x.GetAllAsync(1, It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        _userRepositoryMock
            .Setup(x => x.GetTotalCountAsync(It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        var query = new GetUsersQuery(0, 10); // Page < 1

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Data.Pagination.Page);
        _userRepositoryMock.Verify(
            x => x.GetAllAsync(1, 10, It.IsAny<string?>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}
