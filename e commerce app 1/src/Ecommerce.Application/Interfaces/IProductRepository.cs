using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces;

/// <summary>
/// Repository interface for Product data access.
/// </summary>
public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct = default);
    Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? search, int? categoryId, CancellationToken ct = default);
    Task<Product> AddAsync(Product product, CancellationToken ct = default);
    Task UpdateAsync(Product product, CancellationToken ct = default);
    Task DeleteAsync(Product product, CancellationToken ct = default);
}
