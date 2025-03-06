using System.Diagnostics.CodeAnalysis;

namespace Payment.Application.Common.Models;

public class Result
{
    protected Result()
    {
        IsSuccess = true;
        Error = null;
    }

    protected Result(Error error)
    {
        IsSuccess = false;
        Error = error;
    }
    
    public bool IsSuccess { get; }
    
    [MemberNotNullWhen(false, nameof(IsSuccess))]
    public Error? Error { get; }
    
    public static implicit operator Result(Error error) => new(error);
    
    public static Result Success() => new();

    public static Result Failure(Error error) => new(error);
}

public class Result<TValue> : Result
{
    private Result(TValue? value)
    {
        Value = value;
    }

    private Result(Error error)
        : base(error)
    {
        Value = default;
    }

    public TValue? Value { get; }

    public static implicit operator Result<TValue>(TValue value) => new(value);
    
    public static implicit operator Result<TValue>(Error error) => new(error);

    public static Result<TValue> Success(TValue value) => new(value);

    public new static Result<TValue> Failure(Error error) => new(error);
}