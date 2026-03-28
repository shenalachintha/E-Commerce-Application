using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.DTOs;

/// <summary>
/// DTO for adding item to cart.
/// </summary>
public class AddToCartDto
{
    [Required]
    public int ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}
