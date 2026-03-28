using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Services;

/// <summary>
/// Product service implementing business logic for product operations.
/// </summary>
public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<PagedResultDto<ProductDto>> GetAllProductsAsync(int page = 1, int pageSize = 10, string? search = null, int? categoryId = null, CancellationToken ct = default)
    {
        var (items, totalCount) = await _productRepository.GetPagedAsync(page, pageSize, search, categoryId, ct);
        var dtos = items.Select(MapToDto).ToList();

        return new PagedResultDto<ProductDto>
        {
            Items = dtos,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id, CancellationToken ct = default)
    {
        var product = await _productRepository.GetByIdAsync(id, ct);
        return product == null ? null : MapToDto(product);
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductDto dto, CancellationToken ct = default)
    {
        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            StockQuantity = dto.StockQuantity,
            CategoryId = dto.CategoryId,
            ImageUrl = dto.ImageUrl,
            IsActive = dto.IsActive
        };

        var created = await _productRepository.AddAsync(product, ct);
        return MapToDto(created);
    }

    public async Task<ProductDto?> UpdateProductAsync(int id, UpdateProductDto dto, CancellationToken ct = default)
    {
        var product = await _productRepository.GetByIdAsync(id, ct);
        if (product == null) return null;

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.StockQuantity = dto.StockQuantity;
        product.CategoryId = dto.CategoryId;
        product.ImageUrl = dto.ImageUrl;
        product.IsActive = dto.IsActive;
        product.UpdatedAt = DateTime.UtcNow;

        await _productRepository.UpdateAsync(product, ct);
        return MapToDto(product);
    }

    public async Task<bool> DeleteProductAsync(int id, CancellationToken ct = default)
    {
        var product = await _productRepository.GetByIdAsync(id, ct);
        if (product == null) return false;

        await _productRepository.DeleteAsync(product, ct);
        return true;
    }

    private static ProductDto MapToDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name,
            ImageUrl = product.ImageUrl,
            CreatedAt = product.CreatedAt,
            IsActive = product.IsActive
        };
    }
}
