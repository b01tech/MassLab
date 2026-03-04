using MassLib.Shared.Results;

namespace MassLib.Shared.Tests.Results;

public class ResultTests
{
    [Fact]
    public void Success_ShouldCreateSuccessResult()
    {
        // Act
        var result = Result.Success();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Failure_WithList_ShouldCreateFailureResult()
    {
        // Arrange
        var errors = new List<string> { "Error 1", "Error 2" };

        // Act
        var result = Result.Failure(errors);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(errors, result.Errors);
    }

    [Fact]
    public void Failure_WithSingleError_ShouldCreateFailureResult()
    {
        // Arrange
        var error = "Error message";

        // Act
        var result = Result.Failure(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Single(result.Errors);
        Assert.Equal(error, result.Errors.First());
    }

    [Fact]
    public void MergeErrors_ShouldCombineErrorsFromMultipleResults()
    {
        // Arrange
        var result1 = Result.Failure("Error 1");
        var result2 = Result.Failure(new List<string> { "Error 2", "Error 3" });
        var result3 = Result.Success();

        // Act
        var mergedErrors = Result.MergeErrors(result1, result2, result3);

        // Assert
        Assert.Equal(3, mergedErrors.Count);
        Assert.Contains("Error 1", mergedErrors);
        Assert.Contains("Error 2", mergedErrors);
        Assert.Contains("Error 3", mergedErrors);
    }
}

public class ResultTTests
{
    [Fact]
    public void Success_ShouldCreateSuccessResultWithData()
    {
        // Arrange
        var data = "Test Data";

        // Act
        var result = Result<string>.Success(data);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Empty(result.Errors);
        Assert.Equal(data, result.Data);
    }

    [Fact]
    public void Failure_WithList_ShouldCreateFailureResult()
    {
        // Arrange
        var errors = new List<string> { "Error 1", "Error 2" };

        // Act
        var result = Result<string>.Failure(errors);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(errors, result.Errors);
        Assert.Null(result.Data);
    }

    [Fact]
    public void Failure_WithSingleError_ShouldCreateFailureResult()
    {
        // Arrange
        var error = "Error message";

        // Act
        var result = Result<string>.Failure(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Single(result.Errors);
        Assert.Equal(error, result.Errors.First());
        Assert.Null(result.Data);
    }

    [Fact]
    public void ImplicitOperator_ShouldCreateSuccessResult()
    {
        // Arrange
        var data = 123;

        // Act
        Result<int> result = data;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(data, result.Data);
    }
}
