using MassLab.Shared.Errors;
using MassLab.Shared.ValueObject;

namespace MassLab.Shared.Tests.ValueObjects;

public class CpfTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("123")]
    [InlineData("11111111111")] // All same digits
    [InlineData("12345678900")] // Invalid check digits
    public void Create_ShouldReturnFailure_WhenCpfIsInvalid(string invalidCpf)
    {
        var result = Cpf.Create(invalidCpf);

        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessages.CPF_INVALID, result.Errors);
    }

    [Theory]
    [InlineData("52998224725")]
    [InlineData("529.982.247-25")] // Formatted
    public void Create_ShouldReturnSuccess_WhenCpfIsValid(string validCpf)
    {
        var result = Cpf.Create(validCpf);

        Assert.True(result.IsSuccess);
        Assert.Equal("52998224725", result.Data.Value);
    }
}
