using Ecommerce.Application.DTOs;

namespace Ecommerce.Application.Interfaces;

/// <summary>
/// Service interface for cart operations.
/// </summary>
public interface ICartService
{
    Task<CartDto?> GetCartAsync(int userId, CancellationToken ct = default);
    Task<CartDto> AddToCartAsync(int userId, int productId, int quantity, CancellationToken ct = default);
    Task<CartDto?> UpdateCartItemAsync(int userId, int productId, int quantity, CancellationToken ct = default);
    Task<CartDto?> RemoveFromCartAsync(int userId, int productId, CancellationToken ct = default);
    Task<bool> ClearCartAsync(int userId, CancellationToken ct = default);
}
