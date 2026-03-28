using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.DTOs;

/// <summary>
/// DTO for updating an existing product.
/// </summary>
public class UpdateProductDto
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Url]
    [StringLength(500)]
    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; } = true;
}
