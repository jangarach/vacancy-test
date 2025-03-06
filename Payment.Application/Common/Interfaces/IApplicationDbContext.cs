using System.Data;
using Microsoft.EntityFrameworkCore;
using Payment.Domain.Entities;

namespace Payment.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; set; }

    DbSet<UserPayment> Payments { get; set; }
    
    IDbTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}