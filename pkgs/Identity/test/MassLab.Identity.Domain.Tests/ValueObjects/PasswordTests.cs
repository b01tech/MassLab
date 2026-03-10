using MassLab.Identity.Domain.ValueObjects;
using MassLab.Shared.Errors;

namespace MassLab.Identity.Domain.Tests.ValueObjects;

public class PasswordTests
{
    [Fact]
    public void Create_ShouldReturnFailure_WhenPasswordIsEmpty()
    {
        var result = Password.Create("");

        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessages.PASSWORD_INVALID, result.Errors);
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenPasswordIsTooShort()
    {
        var result = Password.Create("12345");

        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessages.PASSWORD_INVALID, result.Errors);
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenPasswordDoesNotMatchComplexity()
    {
        var result = Password.Create("password"); // No numbers or special chars

        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessages.PASSWORD_INVALID, result.Errors);
    }

    [Fact]
    public void Create_ShouldReturnSuccess_WhenPasswordIsValid()
    {
        var validPassword = "Password123!";

        var result = Password.Create(validPassword);

        Assert.True(result.IsSuccess);
        Assert.Equal(validPassword, result.Data.Value);
    }
}
