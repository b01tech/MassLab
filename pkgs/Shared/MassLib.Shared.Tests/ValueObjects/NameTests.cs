using MassLib.Shared.Errors;
using MassLib.Shared.ValueObject;

namespace MassLib.Shared.Tests.ValueObjects;

public class NameTests
{
    [Fact]
    public void Create_ShouldReturnFailure_WhenNameIsNullOrEmpty()
    {
        // Act
        var result = Name.Create("");

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessages.NAME_INVALID, result.Errors);
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenNameIsTooShort()
    {
        // Act
        var result = Name.Create("Ab");

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessages.NAME_TOO_SHORT, result.Errors);
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenNameIsTooLong()
    {
        // Arrange
        var longName = new string('a', 256);

        // Act
        var result = Name.Create(longName);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessages.NAME_TOO_LONG, result.Errors);
    }

    [Fact]
    public void Create_ShouldReturnSuccess_WhenNameIsValid()
    {
        // Arrange
        var validName = "John Doe";

        // Act
        var result = Name.Create(validName);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(validName, result.Data.Value);
    }

    [Fact]
    public void Create_ShouldTrimName()
    {
        // Arrange
        var nameWithSpaces = "  John Doe  ";

        // Act
        var result = Name.Create(nameWithSpaces);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("John Doe", result.Data.Value);
    }
}
