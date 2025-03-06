using Payment.Domain.Entities;

namespace Payment.Application.Common.Interfaces;

public interface ITokenProvider
{
    public string Create(User user);
}