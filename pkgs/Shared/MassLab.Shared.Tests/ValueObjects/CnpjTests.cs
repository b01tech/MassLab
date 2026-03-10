using MassLab.Shared.Errors;
using MassLab.Shared.ValueObject;

namespace MassLab.Shared.Tests.ValueObjects;

public class CnpjTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("123")]
    [InlineData("11111111111111")] // All same digits
    [InlineData("12345678000190")] // Invalid check digits
    public void Create_ShouldReturnFailure_WhenCnpjIsInvalid(string invalidCnpj)
    {
        var result = Cnpj.Create(invalidCnpj);

        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessages.CNPJ_INVALID, result.Errors);
    }

    [Theory]
    [InlineData("12345678000195")]
    [InlineData("12.345.678/0001-95")] // Formatted
    public void Create_ShouldReturnSuccess_WhenCnpjIsValid(string validCnpj)
    {
        var result = Cnpj.Create(validCnpj);

        Assert.True(result.IsSuccess);
        Assert.Equal("12345678000195", result.Data.Value);
    }
}
