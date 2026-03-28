using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Services;

/// <summary>
/// Category service implementing business logic for category operations.
/// </summary>
public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(CancellationToken ct = default)
    {
        var categories = await _categoryRepository.GetAllAsync(ct);
        return categories.Select(MapToDto);
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(int id, CancellationToken ct = default)
    {
        var category = await _categoryRepository.GetByIdAsync(id, ct);
        return category == null ? null : MapToDto(category);
    }

    public async Task<CategoryDto> CreateCategoryAsync(CategoryDto dto, CancellationToken ct = default)
    {
        var category = new Category
        {
            Name = dto.Name,
            Description = dto.Description,
            ImageUrl = dto.ImageUrl,
            ParentCategoryId = dto.ParentCategoryId,
            IsActive = dto.IsActive
        };

        var created = await _categoryRepository.AddAsync(category, ct);
        return MapToDto(created);
    }

    public async Task<CategoryDto?> UpdateCategoryAsync(int id, CategoryDto dto, CancellationToken ct = default)
    {
        var category = await _categoryRepository.GetByIdAsync(id, ct);
        if (category == null) return null;

        category.Name = dto.Name;
        category.Description = dto.Description;
        category.ImageUrl = dto.ImageUrl;
        category.ParentCategoryId = dto.ParentCategoryId;
        category.IsActive = dto.IsActive;
        category.UpdatedAt = DateTime.UtcNow;

        await _categoryRepository.UpdateAsync(category, ct);
        return MapToDto(category);
    }

    public async Task<bool> DeleteCategoryAsync(int id, CancellationToken ct = default)
    {
        var category = await _categoryRepository.GetByIdAsync(id, ct);
        if (category == null) return false;

        await _categoryRepository.DeleteAsync(category, ct);
        return true;
    }

    private static CategoryDto MapToDto(Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            ImageUrl = category.ImageUrl,
            ParentCategoryId = category.ParentCategoryId,
            CreatedAt = category.CreatedAt,
            IsActive = category.IsActive
        };
    }
}
