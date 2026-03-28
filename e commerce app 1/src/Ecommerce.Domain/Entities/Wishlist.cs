namespace Ecommerce.Domain.Entities;

/// <summary>
/// Wishlist entity for saving products for later.
/// </summary>
public class Wishlist
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;
}
