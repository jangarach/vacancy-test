using MediatR;
using Microsoft.EntityFrameworkCore;
using Payment.Application.Common.Errors;
using Payment.Application.Common.Interfaces;
using Payment.Application.Common.Models;
using Payment.Domain.Entities;

namespace Payment.Application.Commands;

/// <summary>
/// Команда для регистрации пользователя
/// </summary>
/// <param name="Username"> Логин </param>
/// <param name="Password"> Пароль </param>
public record RegisterUserCommand(string Username, string Password) : IRequest<Result<int>>;

/// <summary>
/// Обработчик команды <see cref="RegisterUserCommand"/>
/// </summary>
/// <param name="dbContext"> Контекст базы данных </param>
/// <param name="passwordManager"> Менеджер для работы с пользователями </param>
public class RegisterUserCommandHandler(IApplicationDbContext dbContext, IPasswordManager passwordManager) : IRequestHandler<RegisterUserCommand, Result<int>>
{
    public async Task<Result<int>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await dbContext.Users.AnyAsync(u => u.Username == request.Username, cancellationToken))
        {
            return UserErrors.HasLoginDuplicate();
        }
        
        var user = new User
        {
            Username = request.Username,
            Balance = 8.0m
        };
        
        user.PasswordHash = passwordManager.HashPassword(user, request.Password);
        dbContext.Users.Add(user);
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
