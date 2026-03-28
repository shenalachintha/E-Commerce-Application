using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for User entity.
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(u => u.LastName).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Phone).HasMaxLength(20);
        builder.Property(u => u.Address).HasMaxLength(500);
        builder.Property(u => u.City).HasMaxLength(100);
        builder.Property(u => u.PostalCode).HasMaxLength(20);
        builder.Property(u => u.Country).HasMaxLength(100);
        builder.Property(u => u.Role).IsRequired().HasMaxLength(50);

        builder.HasIndex(u => u.Email).IsUnique();
    }
}
