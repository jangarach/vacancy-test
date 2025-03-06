using Payment.Application.Common.Interfaces;
using Payment.Domain.Entities;

namespace Payment.Infrastructure.Services;

public class TokenProvider : ITokenProvider
{
    public string Create(User user)
    {
        return Guid.NewGuid().ToString();
    }
}