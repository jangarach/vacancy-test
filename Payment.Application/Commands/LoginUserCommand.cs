using MediatR;
using Microsoft.EntityFrameworkCore;
using Payment.Application.Common.Errors;
using Payment.Application.Common.Interfaces;
using Payment.Application.Common.Models;
using Payment.Domain.Entities;

namespace Payment.Application.Commands;

public record LoginUserCommand(string Username, string Password) : IRequest<Result<User>>;

public class LoginUserCommandHandler(
    IApplicationDbContext dbContext,
    IPasswordManager passwordManager,
    ILockoutService lockoutService)
    : IRequestHandler<LoginUserCommand, Result<User>>
{
    public async Task<Result<User>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        if (lockoutService.IsLockedOut(request.Username))
        {
            return UserErrors.TooManyRequests();
        }
        
        var user = await dbContext.Users.SingleOrDefaultAsync(e => e.Username == request.Username, cancellationToken);

        if (user == null || !passwordManager.VerifyHashedPassword(user, request.Password))
        {
            lockoutService.RegisterFailedAttempt(request.Username);
            return UserErrors.Unauthorized();
        }
        
        lockoutService.ResetFailedAttempts(request.Username);
        
        return user;
    }
}