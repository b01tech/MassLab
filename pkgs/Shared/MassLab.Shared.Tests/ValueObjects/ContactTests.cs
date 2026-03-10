using MassLab.Shared.Errors;
using MassLab.Shared.ValueObject;

namespace MassLab.Shared.Tests.ValueObjects;

public class ContactTests
{
    [Fact]
    public void Create_ShouldReturnFailure_WhenNameIsInvalid()
    {
        var result = Contact.Create("", "test@example.com", "11999999999");

        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessages.NAME_INVALID, result.Errors);
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenEmailIsInvalid()
    {
        var result = Contact.Create("John Doe", "invalid", "11999999999");

        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessages.EMAIL_INVALID, result.Errors);
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenPhoneIsInvalid()
    {
        var result = Contact.Create("John Doe", "test@example.com", "123");

        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessages.PHONE_INVALID, result.Errors);
    }

    [Fact]
    public void Create_ShouldReturnSuccess_WhenContactIsValid()
    {
        var result = Contact.Create("  John Doe  ", "test@example.com", "(11) 99999-9999");

        Assert.True(result.IsSuccess);
        Assert.Equal("John Doe", result.Data.Name.Value);
        Assert.Equal("test@example.com", result.Data.Email.Value);
        Assert.Equal("11999999999", result.Data.Phone.Value);
    }
}

