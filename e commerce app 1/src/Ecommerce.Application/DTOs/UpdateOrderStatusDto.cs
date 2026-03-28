using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.DTOs;

/// <summary>
/// DTO for updating order status.
/// </summary>
public class UpdateOrderStatusDto
{
    [Required]
    public string Status { get; set; } = string.Empty; // Pending, Processing, Shipped, Delivered, Cancelled
}
