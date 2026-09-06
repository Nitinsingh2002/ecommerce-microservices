using BuildingBlocks.SharedKernel.Results;

namespace BuildingBlock.SharedKernel.Results;

public sealed class Result<T> : Result
{
    private Result (T value, bool isSucess, Error error) : base (isSucess, error)
    {
        Value = value;
    }

    public T Value { get; }

    public static Result<T> Sucess(T value)
    {
        return new Result<T>(value, true, null);
    }

    public static Result<T> Failure( Error error)
    {
        return new Result<T>(default!, false, error);
    }
}