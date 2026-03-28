using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers;

/// <summary>
/// Categories API controller.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>
    /// GET /api/categories - Get all categories.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var categories = await _categoryService.GetAllCategoriesAsync(ct);
        return Ok(categories);
    }

    /// <summary>
    /// GET /api/categories/{id} - Get category by ID.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id, ct);
        if (category == null) return NotFound();
        return Ok(category);
    }

    /// <summary>
    /// POST /api/categories - Create a new category (admin).
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CategoryDto dto, CancellationToken ct)
    {
        var category = await _categoryService.CreateCategoryAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
    }

    /// <summary>
    /// PUT /api/categories/{id} - Update a category (admin).
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] CategoryDto dto, CancellationToken ct)
    {
        var category = await _categoryService.UpdateCategoryAsync(id, dto, ct);
        if (category == null) return NotFound();
        return Ok(category);
    }

    /// <summary>
    /// DELETE /api/categories/{id} - Delete a category (admin).
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var deleted = await _categoryService.DeleteCategoryAsync(id, ct);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
