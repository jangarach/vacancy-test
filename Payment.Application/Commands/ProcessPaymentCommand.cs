using System.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Payment.Application.Common.Errors;
using Payment.Application.Common.Interfaces;
using Payment.Application.Common.Models;
using Payment.Domain.Entities;

namespace Payment.Application.Commands;

/// <summary>
/// Команда для списания средств с баланса пользователя.
/// </summary>
/// <param name="UserId"> Идентификатор пользователя, с баланса которого будет списана сумма. </param>
public record ProcessPaymentCommand(int UserId) : IRequest<Result>;

/// <summary>
/// Обработчик команды <see cref="ProcessPaymentCommand"/>
/// </summary>
/// <param name="dbContext"> Контекст базы данных </param>
public class ProcessPaymentCommandHandler(
    IApplicationDbContext dbContext, 
    ILogger<ProcessPaymentCommandHandler> logger) 
    : IRequestHandler<ProcessPaymentCommand, Result>
{
    private const decimal Amount = 1.1m;
    
    public async Task<Result> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        using var transaction = dbContext.BeginTransaction(IsolationLevel.Serializable);

        try
        {
            var user = await dbContext.Users.SingleOrDefaultAsync(e => e.Id == request.UserId, cancellationToken: cancellationToken);

            if (user == null)
            {
                return PaymentErrors.NotFoundUser(request.UserId);
            }

            var updatedBalance  = user.Balance - Amount; 

            if (updatedBalance < 0)
            {
                return PaymentErrors.InsufficientFunds(Amount);
            }
        
            user.Balance = updatedBalance;
            dbContext.Users.Update(user);

            var userPayment = new UserPayment
            {
                UserId = user.Id,
                Amount = Amount,
                CreatedAt = DateTimeOffset.UtcNow
            };
        
            dbContext.Payments.Add(userPayment);
        
            await dbContext.SaveChangesAsync(cancellationToken);
        
            transaction.Commit();
        
            return Result.Success();
        }
        catch (Exception e)
        {
            transaction.Rollback();

            logger.LogError(e, "Error while processing payment");
            return PaymentErrors.InternalServerError(e.Message);
        }
    }
}