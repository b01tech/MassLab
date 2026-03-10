using MassLab.Shared.Errors;
using MassLab.Shared.Results;

namespace MassLab.Shared.ValueObject;

public record Cnpj : DocumentNumber
{
    private Cnpj(string value) : base(value) { }

    public static Result<Cnpj> Create(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            return Result<Cnpj>.Failure(ErrorMessages.CNPJ_INVALID);

        var value = new string(cnpj.Where(char.IsDigit).ToArray());

        if (value.Length != 14)
            return Result<Cnpj>.Failure(ErrorMessages.CNPJ_INVALID);

        var multiplier1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        var multiplier2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        if (!IsValidMod11(value, multiplier1, multiplier2))
            return Result<Cnpj>.Failure(ErrorMessages.CNPJ_INVALID);

        return new Cnpj(value);
    }

    public string Formatted => Convert.ToUInt64(Value).ToString(@"00\.000\.000\/0000\-00");
}
