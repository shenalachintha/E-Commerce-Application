using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces;

/// <summary>
/// Repository interface for Category data access.
/// </summary>
public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<Category>> GetAllAsync(CancellationToken ct = default);
    Task<Category> AddAsync(Category category, CancellationToken ct = default);
    Task UpdateAsync(Category category, CancellationToken ct = default);
    Task DeleteAsync(Category category, CancellationToken ct = default);
}
