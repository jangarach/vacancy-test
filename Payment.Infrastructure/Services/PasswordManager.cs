using Microsoft.AspNetCore.Identity;
using Payment.Application.Common.Interfaces;
using Payment.Domain.Entities;

namespace Payment.Infrastructure.Services;

public class PasswordManager : IPasswordManager
{
    public string HashPassword(User user, string password)
    {
        var hashedPassword = new PasswordHasher<User>()
            .HashPassword(user, password);

        return hashedPassword;
    }

    public bool VerifyHashedPassword(User user, string providedPassword)
    {
        var result = new PasswordHasher<User>()
            .VerifyHashedPassword(user, user.PasswordHash, providedPassword);

        return result == PasswordVerificationResult.Success;
    }
}