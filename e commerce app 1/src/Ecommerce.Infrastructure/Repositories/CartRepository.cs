using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories;

/// <summary>
/// Cart repository implementation using Entity Framework Core.
/// </summary>
public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;

    public CartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Cart?> GetByUserIdAsync(int userId, CancellationToken ct = default)
    {
        return await _context.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.UserId == userId, ct);
    }

    public async Task<Cart> CreateAsync(Cart cart, CancellationToken ct = default)
    {
        _context.Carts.Add(cart);
        await _context.SaveChangesAsync(ct);
        return cart;
    }

    public async Task<CartItem?> GetCartItemAsync(int cartId, int productId, CancellationToken ct = default)
    {
        return await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId, ct);
    }

    public async Task<CartItem> AddCartItemAsync(CartItem item, CancellationToken ct = default)
    {
        _context.CartItems.Add(item);
        await _context.SaveChangesAsync(ct);
        return item;
    }

    public async Task UpdateCartItemAsync(CartItem item, CancellationToken ct = default)
    {
        _context.CartItems.Update(item);
        await _context.SaveChangesAsync(ct);
    }

    public async Task RemoveCartItemAsync(CartItem item, CancellationToken ct = default)
    {
        _context.CartItems.Remove(item);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<CartItem>> GetCartItemsAsync(int cartId, CancellationToken ct = default)
    {
        return await _context.CartItems
            .Where(ci => ci.CartId == cartId)
            .ToListAsync(ct);
    }
}
