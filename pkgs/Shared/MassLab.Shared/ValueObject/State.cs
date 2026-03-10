using MassLab.Shared.Errors;
using MassLab.Shared.Results;

namespace MassLab.Shared.ValueObject;

public record State
{
    public string Value { get; }

    private State(string value)
    {
        Value = value;
    }

    private static readonly HashSet<string> ValidStates = new()
    {
        "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA", "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN", "RS", "RO", "RR", "SC", "SP", "SE", "TO"
    };

    public static Result<State> Create(string state)
    {
        if (string.IsNullOrWhiteSpace(state))
            return Result<State>.Failure(ErrorMessages.STATE_INVALID);

        var upperState = state.Trim().ToUpper();

        if (!ValidStates.Contains(upperState))
            return Result<State>.Failure(ErrorMessages.STATE_INVALID);

        return new State(upperState);
    }
    
    public override string ToString() => Value;
}
