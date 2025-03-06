namespace Payment.Application.Common.Models;

public class Error
{
    private Error(ErrorType type, string message)
    {
        Type = type;
        Message = message;
    }
    
    public string Message { get;}
    
    public ErrorType Type { get; }
    
    public static Error InternalServerError(string message) => new(ErrorType.Unknown, message);
    
    public static Error BadRequest(string message) => new(ErrorType.BadRequest, message);

    public static Error Forbidden(string message) => new(ErrorType.Forbidden, message);

    public static Error NotFound(string message) => new(ErrorType.NotFound, message);
    
    public static Error Unauthorized(string message) => new(ErrorType.Unauthorized, message);
    
    public static Error TooManyRequests(string message) => new(ErrorType.TooManyRequests, message);
}