using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payment.Domain.Entities;

namespace Payment.Infrastructure.Database.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<UserPayment>
{
    public void Configure(EntityTypeBuilder<UserPayment> builder)
    {
        builder
            .ToTable("UserPayments");
        
        builder
            .HasKey(u => u.Id);

        builder
            .Property(u => u.Amount)
            .HasPrecision(18, 1);

        builder
            .HasOne(p => p.User)
            .WithMany(u => u.Payments)
            .HasForeignKey(p => p.UserId);
    }
}