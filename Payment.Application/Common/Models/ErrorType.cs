namespace Payment.Application.Common.Models;

public enum ErrorType
{
    Unknown = 0,
    BadRequest = 1,
    Forbidden = 2,
    NotFound = 3,
    Unauthorized = 4,
    TooManyRequests = 5
}