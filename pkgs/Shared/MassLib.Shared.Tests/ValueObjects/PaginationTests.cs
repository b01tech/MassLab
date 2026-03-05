using MassLib.Shared.ValueObject;

namespace MassLib.Shared.Tests.ValueObjects;

public class PaginationTests
{
    [Fact]
    public void Create_ShouldReturnValidPagination_WhenValuesAreValid()
    {
        // Arrange
        int page = 2;
        int pageSize = 50;
        int totalItems = 100;

        // Act
        var pagination = Pagination.Create(page, pageSize, totalItems);

        // Assert
        Assert.Equal(page, pagination.Page);
        Assert.Equal(pageSize, pagination.PageSize);
        Assert.Equal(totalItems, pagination.TotalItems);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_ShouldDefaultPageToOne_WhenPageIsLessThanOne(int invalidPage)
    {
        // Arrange
        int pageSize = 10;
        int totalItems = 100;

        // Act
        var pagination = Pagination.Create(invalidPage, pageSize, totalItems);

        // Assert
        Assert.Equal(1, pagination.Page);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_ShouldDefaultPageSizeToTwentyFive_WhenPageSizeIsLessThanOne(int invalidPageSize)
    {
        // Arrange
        int page = 1;
        int totalItems = 100;

        // Act
        var pagination = Pagination.Create(page, invalidPageSize, totalItems);

        // Assert
        Assert.Equal(25, pagination.PageSize);
    }

    [Fact]
    public void Create_ShouldCapPageSize_WhenExceedsMaxPageSize()
    {
        // Arrange
        int page = 1;
        int excessivePageSize = 201; // Max is 200
        int totalItems = 100;

        // Act
        var pagination = Pagination.Create(page, excessivePageSize, totalItems);

        // Assert
        Assert.Equal(200, pagination.PageSize);
    }

    [Theory]
    [InlineData(10, 10, 1)] // 10 items, 10 per page -> 1 page
    [InlineData(11, 10, 2)] // 11 items, 10 per page -> 2 pages
    [InlineData(0, 10, 0)]  // 0 items -> 0 pages
    [InlineData(25, 5, 5)]  // 25 items, 5 per page -> 5 pages
    public void TotalPages_ShouldCalculateCorrectly(int totalItems, int pageSize, int expectedTotalPages)
    {
        // Arrange
        int page = 1;

        // Act
        var pagination = Pagination.Create(page, pageSize, totalItems);

        // Assert
        Assert.Equal(expectedTotalPages, pagination.TotalPages);
    }
}
