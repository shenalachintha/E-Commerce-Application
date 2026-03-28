using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces;

/// <summary>
/// Repository interface for Cart data access.
/// </summary>
public interface ICartRepository
{
    Task<Cart?> GetByUserIdAsync(int userId, CancellationToken ct = default);
    Task<Cart> CreateAsync(Cart cart, CancellationToken ct = default);
    Task<CartItem?> GetCartItemAsync(int cartId, int productId, CancellationToken ct = default);
    Task<CartItem> AddCartItemAsync(CartItem item, CancellationToken ct = default);
    Task UpdateCartItemAsync(CartItem item, CancellationToken ct = default);
    Task RemoveCartItemAsync(CartItem item, CancellationToken ct = default);
    Task<IEnumerable<CartItem>> GetCartItemsAsync(int cartId, CancellationToken ct = default);
}
