using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers;

/// <summary>
/// Products API controller - supports public read, admin write.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// GET /api/products - Get paginated products with optional search and category filter.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDto<ProductDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] int? categoryId = null,
        CancellationToken ct = default)
    {
        var result = await _productService.GetAllProductsAsync(page, pageSize, search, categoryId, ct);
        return Ok(result);
    }

    /// <summary>
    /// GET /api/products/{id} - Get product by ID.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var product = await _productService.GetProductByIdAsync(id, ct);
        if (product == null) return NotFound();
        return Ok(product);
    }

    /// <summary>
    /// POST /api/products - Create a new product (admin).
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto, CancellationToken ct)
    {
        var product = await _productService.CreateProductAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    /// <summary>
    /// PUT /api/products/{id} - Update a product (admin).
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto, CancellationToken ct)
    {
        var product = await _productService.UpdateProductAsync(id, dto, ct);
        if (product == null) return NotFound();
        return Ok(product);
    }

    /// <summary>
    /// DELETE /api/products/{id} - Delete a product (admin).
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var deleted = await _productService.DeleteProductAsync(id, ct);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
