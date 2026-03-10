using MassLab.Shared.Errors;
using MassLab.Shared.Results;

namespace MassLab.Shared.ValueObject;

public record Cpf : DocumentNumber
{
    private Cpf(string value) : base(value) { }

    public static Result<Cpf> Create(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return Result<Cpf>.Failure(ErrorMessages.CPF_INVALID);

        var value = new string(cpf.Where(char.IsDigit).ToArray());

        if (value.Length != 11)
            return Result<Cpf>.Failure(ErrorMessages.CPF_INVALID);

        if (value.Distinct().Count() == 1)
            return Result<Cpf>.Failure(ErrorMessages.CPF_INVALID);

        var multiplier1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        var multiplier2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        if (!IsValidMod11(value, multiplier1, multiplier2))
            return Result<Cpf>.Failure(ErrorMessages.CPF_INVALID);

        return new Cpf(value);
    }
    
    public string Formatted => Convert.ToUInt64(Value).ToString(@"000\.000\.000\-00");
}
