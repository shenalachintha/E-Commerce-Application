namespace Ecommerce.Domain.Entities;

/// <summary>
/// Order entity representing a customer purchase.
/// </summary>
public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending"; // Pending, Processing, Shipped, Delivered, Cancelled
    public decimal SubTotal { get; set; }
    public decimal Tax { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal Total { get; set; }
    public string ShippingAddress { get; set; } = string.Empty;
    public string? BillingAddress { get; set; }
    public string? Notes { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public DateTime? ShippedDate { get; set; }
    public DateTime? DeliveredDate { get; set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
