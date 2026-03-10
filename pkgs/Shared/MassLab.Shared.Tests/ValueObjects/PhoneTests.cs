using MassLab.Shared.Errors;
using MassLab.Shared.ValueObject;

namespace MassLab.Shared.Tests.ValueObjects;

public class PhoneTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("123")]
    [InlineData("123456789")] // Too short
    [InlineData("123456789012")] // Too long
    public void Create_ShouldReturnFailure_WhenPhoneIsInvalid(string invalidPhone)
    {
        var result = Phone.Create(invalidPhone);

        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessages.PHONE_INVALID, result.Errors);
    }

    [Theory]
    [InlineData("11999999999")]
    [InlineData("(11) 99999-9999")]
    [InlineData("1133334444")]
    [InlineData("(11) 3333-4444")]
    public void Create_ShouldReturnSuccess_WhenPhoneIsValid(string validPhone)
    {
        var result = Phone.Create(validPhone);

        Assert.True(result.IsSuccess);
    }
}
