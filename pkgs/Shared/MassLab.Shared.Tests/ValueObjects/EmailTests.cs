using MassLab.Shared.Errors;
using MassLab.Shared.ValueObject;

namespace MassLab.Shared.Tests.ValueObjects;

public class EmailTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("invalid")]
    [InlineData("invalid@")]
    [InlineData("@invalid.com")]
    [InlineData("test@com")] // Missing domain part
    public void Create_ShouldReturnFailure_WhenEmailIsInvalid(string invalidEmail)
    {
        var result = Email.Create(invalidEmail);

        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessages.EMAIL_INVALID, result.Errors);
    }

    [Theory]
    [InlineData("test@example.com")]
    [InlineData("user.name@domain.co.uk")]
    public void Create_ShouldReturnSuccess_WhenEmailIsValid(string validEmail)
    {
        var result = Email.Create(validEmail);

        Assert.True(result.IsSuccess);
        Assert.Equal(validEmail, result.Data.Value);
    }
}
