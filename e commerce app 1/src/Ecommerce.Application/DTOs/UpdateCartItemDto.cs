using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.DTOs;

/// <summary>
/// DTO for updating cart item quantity.
/// </summary>
public class UpdateCartItemDto
{
    [Required]
    public int ProductId { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
}
