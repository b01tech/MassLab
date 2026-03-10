using MassLab.Shared.Errors;
using MassLab.Shared.ValueObject;

namespace MassLab.Shared.Tests.ValueObjects;

public class AddressTests
{
    [Fact]
    public void Create_ShouldReturnFailure_WhenAnyRequiredFieldIsInvalid()
    {
        // Invalid Street
        var result = Address.Create("", "123", "", "Nb", "12345678", "City", "SP");
        Assert.True(result.IsFailure);
        
        // Invalid ZipCode
        result = Address.Create("Street", "123", "", "Nb", "123", "City", "SP");
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessages.ZIPCODE_INVALID, result.Errors);

        // Invalid State
        result = Address.Create("Street", "123", "", "Nb", "12345678", "City", "ZZ");
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessages.STATE_INVALID, result.Errors);
    }

    [Fact]
    public void Create_ShouldReturnSuccess_WhenAddressIsValid()
    {
        var result = Address.Create("Street", "123", "Apt 1", "Downtown", "12345678", "Sao Paulo", "SP");

        Assert.True(result.IsSuccess);
        Assert.Equal("Street", result.Data.Street);
        Assert.Equal("123", result.Data.Number);
        Assert.Equal("Apt 1", result.Data.Complement);
        Assert.Equal("Downtown", result.Data.Neighborhood);
        Assert.Equal("12345678", result.Data.ZipCode);
        Assert.Equal("Sao Paulo", result.Data.City.Name);
        Assert.Equal("SP", result.Data.State.Value);
    }
}
