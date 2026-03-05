using MassLib.Shared.Errors;
using MassLib.Shared.Results;

namespace MassLib.Shared.ValueObject;

public record Name
{
    public string Value { get; }
    private const int MinLength = 3;
    private const int MaxLength = 255;

    private Name(string value)
    {
        Value = value;
    }

    public static Result<Name> Create(string name, int? min = null, int? max = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<Name>.Failure(ErrorMessages.NAME_INVALID);
        var formatedName = name.Trim();
        if (formatedName.Length < (min ?? MinLength))
            return Result<Name>.Failure(ErrorMessages.NAME_TOO_SHORT);
        if (formatedName.Length > (max ?? MaxLength))
            return Result<Name>.Failure(ErrorMessages.NAME_TOO_LONG);

        return new Name(formatedName);
    }

    public override string ToString() => Value;
};
