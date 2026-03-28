using System.Security.Claims;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers;

/// <summary>
/// Cart API controller - requires authentication.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

    /// <summary>
    /// GET /api/cart - Get current user's cart.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(CartDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCart(CancellationToken ct)
    {
        var cart = await _cartService.GetCartAsync(UserId, ct);
        if (cart == null) return Ok(new CartDto { UserId = UserId, Items = new List<CartItemDto>() });
        return Ok(cart);
    }

    /// <summary>
    /// POST /api/cart - Add item to cart.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CartDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartDto dto, CancellationToken ct)
    {
        try
        {
            var cart = await _cartService.AddToCartAsync(UserId, dto.ProductId, dto.Quantity, ct);
            return Ok(cart);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// PUT /api/cart - Update cart item quantity.
    /// </summary>
    [HttpPut]
    [ProducesResponseType(typeof(CartDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemDto dto, CancellationToken ct)
    {
        try
        {
            var cart = await _cartService.UpdateCartItemAsync(UserId, dto.ProductId, dto.Quantity, ct);
            if (cart == null) return NotFound();
            return Ok(cart);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// DELETE /api/cart/{productId} - Remove item from cart.
    /// </summary>
    [HttpDelete("{productId:int}")]
    [ProducesResponseType(typeof(CartDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveFromCart(int productId, CancellationToken ct)
    {
        var cart = await _cartService.RemoveFromCartAsync(UserId, productId, ct);
        if (cart == null) return NotFound();
        return Ok(cart);
    }

    /// <summary>
    /// DELETE /api/cart - Clear entire cart.
    /// </summary>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ClearCart(CancellationToken ct)
    {
        await _cartService.ClearCartAsync(UserId, ct);
        return NoContent();
    }
}
