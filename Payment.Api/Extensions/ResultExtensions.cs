using Microsoft.AspNetCore.Http.HttpResults;
using Payment.Application.Common.Models;

namespace Payment.Api.Extensions;

public static class ResultExtensions
{
    public static Results<Ok, ProblemHttpResult> CreateResponse(this Result result)
    {
        if (result.IsSuccess)
        {
            return TypedResults.Ok();
        }

        return result.CreateProblem();
    }
    
    public static Results<Ok<T>, ProblemHttpResult> CreateResponse<T>(this Result<T>  result)
    {
        if (result.IsSuccess)
        {
            return TypedResults.Ok(result.Value);
        }

        return result.CreateProblem();
    }
    
    public static ProblemHttpResult CreateProblem(this Result result)
    {
        var statusCode = result.Error!.Type switch
        {
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.BadRequest => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.TooManyRequests => StatusCodes.Status429TooManyRequests,
            ErrorType.Unknown => StatusCodes.Status500InternalServerError,
            _ => throw new ArgumentOutOfRangeException()
        };

        return TypedResults.Problem(result.Error.Message, statusCode: statusCode);
    }
}