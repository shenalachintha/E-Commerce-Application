using Ecommerce.Application.DTOs;

namespace Ecommerce.Application.Interfaces;

/// <summary>
/// Service interface for order operations.
/// </summary>
public interface IOrderService
{
    Task<OrderDto?> GetOrderByIdAsync(int id, int? userId = null, CancellationToken ct = default);
    Task<OrderDto?> GetOrderByOrderNumberAsync(string orderNumber, int? userId = null, CancellationToken ct = default);
    Task<PagedResultDto<OrderDto>> GetUserOrderHistoryAsync(int userId, int page = 1, int pageSize = 10, CancellationToken ct = default);
    Task<OrderDto> CreateOrderAsync(int userId, string shippingAddress, string? billingAddress, string? notes, CancellationToken ct = default);
    Task<OrderDto?> UpdateOrderStatusAsync(int id, string status, int? userId = null, CancellationToken ct = default);
}
