using Ecommerce.Application.DTOs;

namespace Ecommerce.Application.Interfaces;

/// <summary>
/// Service interface for category operations.
/// </summary>
public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(CancellationToken ct = default);
    Task<CategoryDto?> GetCategoryByIdAsync(int id, CancellationToken ct = default);
    Task<CategoryDto> CreateCategoryAsync(CategoryDto dto, CancellationToken ct = default);
    Task<CategoryDto?> UpdateCategoryAsync(int id, CategoryDto dto, CancellationToken ct = default);
    Task<bool> DeleteCategoryAsync(int id, CancellationToken ct = default);
}
