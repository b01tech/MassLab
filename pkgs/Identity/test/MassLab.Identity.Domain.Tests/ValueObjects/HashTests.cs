using MassLab.Identity.Domain.ValueObjects;
using MassLab.Shared.Errors;

namespace MassLab.Identity.Domain.Tests.ValueObjects;

public class HashTests
{
    [Fact]
    public void Create_ShouldReturnFailure_WhenHashIsEmpty()
    {
        var result = Hash.Create("");

        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessages.HASH_INVALID, result.Errors);
    }

    [Fact]
    public void Create_ShouldReturnSuccess_WhenHashIsValid()
    {
        var validHash = "some_valid_hash_string";

        var result = Hash.Create(validHash);

        Assert.True(result.IsSuccess);
        Assert.Equal(validHash, result.Data.Value);
    }
}
