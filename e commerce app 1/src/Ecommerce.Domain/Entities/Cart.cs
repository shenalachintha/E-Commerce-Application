namespace Ecommerce.Domain.Entities;

/// <summary>
/// Cart entity representing a user's shopping cart.
/// </summary>
public class Cart
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
