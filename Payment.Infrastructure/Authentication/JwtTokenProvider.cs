using System.Text;
using Microsoft.Extensions.Configuration;
using Payment.Domain.Entities;

namespace Payment.Infrastructure.Authentication;

internal sealed class JwtTokenProvider(IConfiguration configuration)
{
    public string Create(User user)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]!);
        return string.Empty;
    }
}