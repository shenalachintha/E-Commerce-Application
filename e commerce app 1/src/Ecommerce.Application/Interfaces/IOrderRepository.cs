using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces;

/// <summary>
/// Repository interface for Order data access.
/// </summary>
public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken ct = default);
    Task<IEnumerable<Order>> GetByUserIdAsync(int userId, CancellationToken ct = default);
    Task<(IEnumerable<Order> Items, int TotalCount)> GetByUserIdPagedAsync(int userId, int page, int pageSize, CancellationToken ct = default);
    Task<Order> AddAsync(Order order, CancellationToken ct = default);
    Task UpdateAsync(Order order, CancellationToken ct = default);
}
