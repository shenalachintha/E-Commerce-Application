namespace Ecommerce.Domain.Entities;

/// <summary>
/// CartItem entity representing a product in a shopping cart.
/// </summary>
public class CartItem
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Cart Cart { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;
}
