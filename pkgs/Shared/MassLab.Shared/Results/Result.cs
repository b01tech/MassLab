namespace MassLab.Shared.Results;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public IList<string> Errors { get; }

    protected Result(bool isSuccess, IList<string> errors)
    {
        IsSuccess = isSuccess;
        Errors = new List<string>(errors);
    }
    public static Result Success() => new Result(true, []);
    public static Result Failure(IList<string> errors) => new Result(false, errors);
    public static Result Failure(string error) => new Result(false, [error]);
    public static IList<string> MergeErrors(params Result[] results) => results.SelectMany(r => r.Errors).ToList();

}
public class Result<T> : Result
{
    public T Data { get; } = default!;
    private Result(bool isSuccess, IList<string> errors, T data)
        : base(isSuccess, errors)
    {
        Data = data;
    }
    public static Result<T> Success(T data) => new Result<T>(true, [], data);
    public static new Result<T> Failure(IList<string> errors) => new(false, errors, default!);
    public static new Result<T> Failure(string error) => new(false, [error], default!);

    public static implicit operator Result<T>(T data) => Success(data);
}
