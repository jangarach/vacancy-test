using Payment.Domain.Entities;

namespace Payment.Application.Common.Interfaces;

public interface IPasswordManager
{
    string HashPassword(User user, string password);
    
    bool VerifyHashedPassword(User user, string providedPassword);
}