namespace Ecommerce.Application.DTOs;

/// <summary>
/// Data transfer object for Category.
/// </summary>
public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public int? ParentCategoryId { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
}
