using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Services;

/// <summary>
/// Cart service implementing business logic for cart operations.
/// </summary>
public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public CartService(ICartRepository cartRepository, IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }

    public async Task<CartDto?> GetCartAsync(int userId, CancellationToken ct = default)
    {
        var cart = await _cartRepository.GetByUserIdAsync(userId, ct);
        if (cart == null) return null;

        return await MapToDtoAsync(cart, ct);
    }

    public async Task<CartDto> AddToCartAsync(int userId, int productId, int quantity, CancellationToken ct = default)
    {
        var product = await _productRepository.GetByIdAsync(productId, ct);
        if (product == null || product.StockQuantity < quantity)
            throw new InvalidOperationException("Product not found or insufficient stock.");

        var cart = await _cartRepository.GetByUserIdAsync(userId, ct);
        if (cart == null)
        {
            cart = await _cartRepository.CreateAsync(new Cart { UserId = userId }, ct);
        }

        var existingItem = await _cartRepository.GetCartItemAsync(cart.Id, productId, ct);
        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
            existingItem.UnitPrice = product.Price;
            await _cartRepository.UpdateCartItemAsync(existingItem, ct);
        }
        else
        {
            await _cartRepository.AddCartItemAsync(new CartItem
            {
                CartId = cart.Id,
                ProductId = productId,
                Quantity = quantity,
                UnitPrice = product.Price
            }, ct);
        }

        var updatedCart = await _cartRepository.GetByUserIdAsync(userId, ct)!;
        return await MapToDtoAsync(updatedCart!, ct);
    }

    public async Task<CartDto?> UpdateCartItemAsync(int userId, int productId, int quantity, CancellationToken ct = default)
    {
        var cart = await _cartRepository.GetByUserIdAsync(userId, ct);
        if (cart == null) return null;

        var item = await _cartRepository.GetCartItemAsync(cart.Id, productId, ct);
        if (item == null) return null;

        var product = await _productRepository.GetByIdAsync(productId, ct);
        if (product != null && product.StockQuantity < quantity)
            throw new InvalidOperationException("Insufficient stock.");

        item.Quantity = quantity;
        item.UnitPrice = product?.Price ?? item.UnitPrice;
        await _cartRepository.UpdateCartItemAsync(item, ct);

        var updatedCart = await _cartRepository.GetByUserIdAsync(userId, ct);
        return updatedCart == null ? null : await MapToDtoAsync(updatedCart, ct);
    }

    public async Task<CartDto?> RemoveFromCartAsync(int userId, int productId, CancellationToken ct = default)
    {
        var cart = await _cartRepository.GetByUserIdAsync(userId, ct);
        if (cart == null) return null;

        var items = await _cartRepository.GetCartItemsAsync(cart.Id, ct);
        var item = items.FirstOrDefault(i => i.ProductId == productId);
        if (item == null) return null;

        await _cartRepository.RemoveCartItemAsync(item, ct);

        var updatedCart = await _cartRepository.GetByUserIdAsync(userId, ct);
        return updatedCart == null ? null : await MapToDtoAsync(updatedCart, ct);
    }

    public async Task<bool> ClearCartAsync(int userId, CancellationToken ct = default)
    {
        var cart = await _cartRepository.GetByUserIdAsync(userId, ct);
        if (cart == null) return true;

        var items = await _cartRepository.GetCartItemsAsync(cart.Id, ct);
        foreach (var item in items)
        {
            await _cartRepository.RemoveCartItemAsync(item, ct);
        }
        return true;
    }

    private async Task<CartDto> MapToDtoAsync(Cart cart, CancellationToken ct)
    {
        var items = await _cartRepository.GetCartItemsAsync(cart.Id, ct);
        var itemDtos = new List<CartItemDto>();
        decimal total = 0;

        foreach (var item in items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, ct);
            var lineTotal = item.Quantity * item.UnitPrice;
            total += lineTotal;
            itemDtos.Add(new CartItemDto
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductName = product?.Name ?? "Unknown",
                ProductImageUrl = product?.ImageUrl,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                LineTotal = lineTotal
            });
        }

        return new CartDto
        {
            Id = cart.Id,
            UserId = cart.UserId,
            Total = total,
            Items = itemDtos
        };
    }
}
