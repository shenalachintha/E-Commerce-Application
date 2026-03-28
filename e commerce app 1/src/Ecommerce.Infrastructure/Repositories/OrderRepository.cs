using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories;

/// <summary>
/// Order repository implementation using Entity Framework Core.
/// </summary>
public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id, ct);
    }

    public async Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken ct = default)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber, ct);
    }

    public async Task<IEnumerable<Order>> GetByUserIdAsync(int userId, CancellationToken ct = default)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(ct);
    }

    public async Task<(IEnumerable<Order> Items, int TotalCount)> GetByUserIdPagedAsync(int userId, int page, int pageSize, CancellationToken ct = default)
    {
        var query = _context.Orders
            .Include(o => o.OrderItems)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    public async Task<Order> AddAsync(Order order, CancellationToken ct = default)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync(ct);
        return order;
    }

    public async Task UpdateAsync(Order order, CancellationToken ct = default)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync(ct);
    }
}
