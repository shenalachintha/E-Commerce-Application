using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories;

/// <summary>
/// Category repository implementation using Entity Framework Core.
/// </summary>
public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Category?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Categories.FindAsync([id], ct);
    }

    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Categories
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync(ct);
    }

    public async Task<Category> AddAsync(Category category, CancellationToken ct = default)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync(ct);
        return category;
    }

    public async Task UpdateAsync(Category category, CancellationToken ct = default)
    {
        category.UpdatedAt = DateTime.UtcNow;
        _context.Categories.Update(category);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Category category, CancellationToken ct = default)
    {
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync(ct);
    }
}
