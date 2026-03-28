using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for Wishlist entity.
/// </summary>
public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
{
    public void Configure(EntityTypeBuilder<Wishlist> builder)
    {
        builder.ToTable("Wishlists");

        builder.HasKey(w => w.Id);

        builder.HasOne(w => w.User)
            .WithMany(u => u.Wishlists)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(w => w.Product)
            .WithMany(p => p.Wishlists)
            .HasForeignKey(w => w.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(w => new { w.UserId, w.ProductId }).IsUnique();
    }
}
