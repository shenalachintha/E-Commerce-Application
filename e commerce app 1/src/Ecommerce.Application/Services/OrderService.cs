using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Services;

/// <summary>
/// Order service implementing business logic for order operations.
/// </summary>
public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }

    public async Task<OrderDto?> GetOrderByIdAsync(int id, int? userId = null, CancellationToken ct = default)
    {
        var order = await _orderRepository.GetByIdAsync(id, ct);
        if (order == null) return null;
        if (userId.HasValue && order.UserId != userId.Value) return null;

        return MapToDto(order);
    }

    public async Task<OrderDto?> GetOrderByOrderNumberAsync(string orderNumber, int? userId = null, CancellationToken ct = default)
    {
        var order = await _orderRepository.GetByOrderNumberAsync(orderNumber, ct);
        if (order == null) return null;
        if (userId.HasValue && order.UserId != userId.Value) return null;

        return MapToDto(order);
    }

    public async Task<PagedResultDto<OrderDto>> GetUserOrderHistoryAsync(int userId, int page = 1, int pageSize = 10, CancellationToken ct = default)
    {
        var (orders, totalCount) = await _orderRepository.GetByUserIdPagedAsync(userId, page, pageSize, ct);
        var dtos = orders.Select(MapToDto).ToList();

        return new PagedResultDto<OrderDto>
        {
            Items = dtos,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<OrderDto> CreateOrderAsync(int userId, string shippingAddress, string? billingAddress, string? notes, CancellationToken ct = default)
    {
        var cart = await _cartRepository.GetByUserIdAsync(userId, ct);
        if (cart == null)
            throw new InvalidOperationException("Cart is empty.");

        var cartItems = await _cartRepository.GetCartItemsAsync(cart.Id, ct);
        if (!cartItems.Any())
            throw new InvalidOperationException("Cart is empty.");

        decimal subTotal = 0;
        var orderItems = new List<OrderItem>();

        foreach (var item in cartItems)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, ct);
            if (product == null || product.StockQuantity < item.Quantity)
                throw new InvalidOperationException($"Insufficient stock for product: {product?.Name ?? "Unknown"}");

            var lineTotal = item.Quantity * item.UnitPrice;
            subTotal += lineTotal;
            orderItems.Add(new OrderItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            });

            // Deduct stock
            product.StockQuantity -= item.Quantity;
            product.UpdatedAt = DateTime.UtcNow;
        }

        var tax = subTotal * 0.1m;
        var shippingCost = subTotal > 100 ? 0 : 9.99m;
        var total = subTotal + tax + shippingCost;
        var orderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";

        var order = new Order
        {
            UserId = userId,
            OrderNumber = orderNumber,
            Status = "Pending",
            SubTotal = subTotal,
            Tax = tax,
            ShippingCost = shippingCost,
            Total = total,
            ShippingAddress = shippingAddress,
            BillingAddress = billingAddress ?? shippingAddress,
            Notes = notes,
            OrderItems = orderItems
        };

        var created = await _orderRepository.AddAsync(order, ct);

        // Clear cart
        foreach (var item in cartItems)
        {
            await _cartRepository.RemoveCartItemAsync(item, ct);
        }

        return MapToDto(created);
    }

    public async Task<OrderDto?> UpdateOrderStatusAsync(int id, string status, int? userId = null, CancellationToken ct = default)
    {
        var order = await _orderRepository.GetByIdAsync(id, ct);
        if (order == null) return null;
        if (userId.HasValue && order.UserId != userId.Value) return null;

        var validStatuses = new[] { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" };
        if (!validStatuses.Contains(status))
            throw new ArgumentException($"Invalid status. Must be one of: {string.Join(", ", validStatuses)}");

        order.Status = status;
        if (status == "Shipped") order.ShippedDate = DateTime.UtcNow;
        if (status == "Delivered") order.DeliveredDate = DateTime.UtcNow;

        await _orderRepository.UpdateAsync(order, ct);
        return MapToDto(order);
    }

    private static OrderDto MapToDto(Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            UserId = order.UserId,
            OrderNumber = order.OrderNumber,
            Status = order.Status,
            SubTotal = order.SubTotal,
            Tax = order.Tax,
            ShippingCost = order.ShippingCost,
            Total = order.Total,
            ShippingAddress = order.ShippingAddress,
            OrderDate = order.OrderDate,
            ShippedDate = order.ShippedDate,
            DeliveredDate = order.DeliveredDate,
            OrderItems = order.OrderItems.Select(oi => new OrderItemDto
            {
                Id = oi.Id,
                ProductId = oi.ProductId,
                ProductName = oi.ProductName,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice
            }).ToList()
        };
    }
}
