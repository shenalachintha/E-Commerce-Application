using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories;

/// <summary>
/// Product repository implementation using Entity Framework Core.
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync(ct);
    }

    public async Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? search, int? categoryId, CancellationToken ct = default)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .Where(p => p.IsActive);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p =>
                p.Name.Contains(search) ||
                p.Description.Contains(search));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .OrderBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    public async Task<Product> AddAsync(Product product, CancellationToken ct = default)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync(ct);
        return product;
    }

    public async Task UpdateAsync(Product product, CancellationToken ct = default)
    {
        product.UpdatedAt = DateTime.UtcNow;
        _context.Products.Update(product);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Product product, CancellationToken ct = default)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync(ct);
    }
}
