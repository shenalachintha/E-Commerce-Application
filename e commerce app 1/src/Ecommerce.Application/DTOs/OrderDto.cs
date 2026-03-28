namespace Ecommerce.Application.DTOs;

/// <summary>
/// Data transfer object for Order.
/// </summary>
public class OrderDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal SubTotal { get; set; }
    public decimal Tax { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal Total { get; set; }
    public string ShippingAddress { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public DateTime? ShippedDate { get; set; }
    public DateTime? DeliveredDate { get; set; }
    public List<OrderItemDto> OrderItems { get; set; } = new();
}

public class OrderItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
