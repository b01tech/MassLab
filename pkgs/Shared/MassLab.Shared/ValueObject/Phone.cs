using System.Linq;
using System.Text.RegularExpressions;
using MassLab.Shared.Errors;
using MassLab.Shared.Results;

namespace MassLab.Shared.ValueObject;

public record Phone
{
    public string Value { get; }

    private Phone(string value)
    {
        Value = value;
    }

    public static Result<Phone> Create(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return Result<Phone>.Failure(ErrorMessages.PHONE_INVALID);

        // Remove non-digits
        var digits = new string(phone.Where(char.IsDigit).ToArray());

        // Check length: 10 (landline) or 11 (mobile)
        if (digits.Length < 10 || digits.Length > 11)
            return Result<Phone>.Failure(ErrorMessages.PHONE_INVALID);

        return new Phone(digits);
    }

    public override string ToString()
    {
        if (Value.Length == 11)
            return Convert.ToUInt64(Value).ToString(@"(00) 00000-0000");
        return Convert.ToUInt64(Value).ToString(@"(00) 0000-0000");
    }
}
