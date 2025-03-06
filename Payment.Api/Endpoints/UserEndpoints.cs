using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Payment.Api.Extensions;
using Payment.Application.Commands;

namespace Payment.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var usersGroup = endpoints.MapGroup("users");

        usersGroup.MapPost("login", Login);
        
        usersGroup.MapPost("logout", Logout)
            .RequireAuthorization();
        
        usersGroup
            .MapPost("payment", PaymentProcess)
            .RequireAuthorization();
    }
    
    private static async Task<Results<Ok, ProblemHttpResult>> Login(
        [FromBody] LoginUserCommand command,
        IMediator mediator,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return result.CreateProblem();
        }
        
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, result.Value!.Username),
            new(ClaimTypes.NameIdentifier, result.Value!.Id.ToString())
        };
        
        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            IssuedUtc = DateTimeOffset.UtcNow
        };

        await httpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme, 
            new ClaimsPrincipal(claimsIdentity), 
            authProperties);

        return TypedResults.Ok();
    }
    
    private static async Task<Results<Ok, ProblemHttpResult>> Logout(
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return TypedResults.Ok();
    }

    private static async Task<Results<Ok, ProblemHttpResult>> PaymentProcess(
        IMediator mediator,
        ClaimsPrincipal claimsPrincipal,
        CancellationToken cancellationToken)
    {
        var userIdString = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdString, out var userId))
        {
            return TypedResults.Problem(detail: "Not a valid user id",  statusCode: StatusCodes.Status400BadRequest);
        }
        
        var result = await mediator.Send(new ProcessPaymentCommand(userId), cancellationToken);
        return result.CreateResponse();
    }
}