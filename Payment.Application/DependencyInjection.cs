using Microsoft.Extensions.DependencyInjection;
using Payment.Application.Common.Interfaces;
using Payment.Application.Common.Services;

namespace Payment.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;
        services.AddMediatR(config =>
            config.RegisterServicesFromAssembly(assembly));

        services.AddScoped<ILockoutService, LockoutService>();
        
        return services;
    }
}