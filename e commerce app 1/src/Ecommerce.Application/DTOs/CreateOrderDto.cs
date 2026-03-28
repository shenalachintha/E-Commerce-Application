using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.DTOs;

/// <summary>
/// DTO for creating an order from cart.
/// </summary>
public class CreateOrderDto
{
    [Required]
    public string ShippingAddress { get; set; } = string.Empty;

    public string? BillingAddress { get; set; }

    public string? Notes { get; set; }
}
