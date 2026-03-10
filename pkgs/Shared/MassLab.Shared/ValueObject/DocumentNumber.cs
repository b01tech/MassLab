using System.Linq;

namespace MassLab.Shared.ValueObject;

public abstract record DocumentNumber
{
    public string Value { get; init; }

    protected DocumentNumber(string value)
    {
        Value = value;
    }

    public override string ToString() => Value;

    // Helper for check digit calculation (Mod11)
    protected static bool IsValidMod11(string number, int[] multipliers1, int[] multipliers2)
    {
        if (number.Distinct().Count() == 1) return false;

        var tempCnpj = number.Substring(0, multipliers1.Length);
        var soma = 0;

        for (int i = 0; i < multipliers1.Length; i++)
            soma += int.Parse(tempCnpj[i].ToString()) * multipliers1[i];

        var resto = (soma % 11);
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        var digito = resto.ToString();
        tempCnpj = tempCnpj + digito;
        soma = 0;

        for (int i = 0; i < multipliers2.Length; i++)
            soma += int.Parse(tempCnpj[i].ToString()) * multipliers2[i];

        resto = (soma % 11);
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        digito = digito + resto.ToString();

        return number.EndsWith(digito);
    }
}
