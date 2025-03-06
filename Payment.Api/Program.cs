using Microsoft.AspNetCore.Authentication.Cookies;
using Payment.Api.Endpoints;
using Payment.Api.Extensions;
using Payment.Application;
using Payment.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.Cookie.Name = "Cookie";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
        options.SlidingExpiration = false;
        options.Events.OnRedirectToLogin = c =>
        {
            c.Response.StatusCode = 401;
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddMemoryCache();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(endpointPrefix: string.Empty);
    await app.ApplyMigrationsAsync();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapUserEndpoints();
app.UseHttpsRedirection();
app.Run();