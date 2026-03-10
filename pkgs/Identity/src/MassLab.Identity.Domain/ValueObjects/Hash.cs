using System.Text.RegularExpressions;
using MassLab.Shared.Errors;
using MassLab.Shared.Results;

namespace MassLab.Identity.Domain.ValueObjects;

public record Hash
{
    public string Value { get; }

    private Hash(string value)
    {
        Value = value;
    }

    public static Result<Hash> Create(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            return Result<Hash>.Failure(ErrorMessages.HASH_INVALID);

        return new Hash(hash);
    }

    public override string ToString() => Value;
};
