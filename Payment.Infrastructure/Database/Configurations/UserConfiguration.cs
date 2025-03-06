using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payment.Domain.Entities;
using Payment.Infrastructure.Services;

namespace Payment.Infrastructure.Database.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .ToTable("User");
        
        builder
            .HasKey(p => p.Id);
        
        builder
            .HasIndex(u => u.Username)
            .IsUnique();

        builder
            .Property(u => u.Balance)
            .HasPrecision(18, 2);

        builder.HasData(CreateDefaultUser());
    }

    private static User CreateDefaultUser()
    {
        var user = new User
        {
            Id = 1,
            Username = "admin",
            Balance = 8.0m
        };

        user.PasswordHash = new PasswordManager()
            .HashPassword(user, "admin");
        
        return user;
    }
}