namespace Ecommerce.Domain.Entities;

/// <summary>
/// OrderItem entity representing a product in an order.
/// </summary>
public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string ProductName { get; set; } = string.Empty;

    // Navigation properties
    public virtual Order Order { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;
}
