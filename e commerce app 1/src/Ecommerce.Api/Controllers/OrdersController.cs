using System.Security.Claims;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers;

/// <summary>
/// Orders API controller - requires authentication.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

    /// <summary>
    /// GET /api/orders/history - Get user's order history (paginated).
    /// </summary>
    [HttpGet("history")]
    [ProducesResponseType(typeof(PagedResultDto<OrderDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrderHistory(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        var result = await _orderService.GetUserOrderHistoryAsync(UserId, page, pageSize, ct);
        return Ok(result);
    }

    /// <summary>
    /// GET /api/orders/{id} - Get order by ID.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var order = await _orderService.GetOrderByIdAsync(id, UserId, ct);
        if (order == null) return NotFound();
        return Ok(order);
    }

    /// <summary>
    /// GET /api/orders/number/{orderNumber} - Get order by order number.
    /// </summary>
    [HttpGet("number/{orderNumber}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByOrderNumber(string orderNumber, CancellationToken ct)
    {
        var order = await _orderService.GetOrderByOrderNumberAsync(orderNumber, UserId, ct);
        if (order == null) return NotFound();
        return Ok(order);
    }

    /// <summary>
    /// POST /api/orders - Create order from cart (checkout).
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateOrderDto dto, CancellationToken ct)
    {
        try
        {
            var order = await _orderService.CreateOrderAsync(
                UserId,
                dto.ShippingAddress,
                dto.BillingAddress,
                dto.Notes,
                ct);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// PUT /api/orders/{id}/status - Update order status. Admin can update any order.
    /// </summary>
    [HttpPut("{id:int}/status")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateOrderStatusDto dto, CancellationToken ct)
    {
        try
        {
            var userId = User.IsInRole("Admin") ? null : (int?)UserId;
            var order = await _orderService.UpdateOrderStatusAsync(id, dto.Status, userId, ct);
            if (order == null) return NotFound();
            return Ok(order);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
