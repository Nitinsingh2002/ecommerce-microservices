
namespace BuildingBlocks.SharedKernel.Results;


public class Result
{
    protected Result(bool isSucess, Error? error)
    {
        IsSuccess = isSucess;
        Error = error;
    }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error? Error { get; }

    public static Result Sucess()
    {
        return new Result(true, null);
    }
    
    public static Result Failure(Error error)
    {
        return new Result(false, error);
    }

}