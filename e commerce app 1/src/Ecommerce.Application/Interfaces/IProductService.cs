using Ecommerce.Application.DTOs;

namespace Ecommerce.Application.Interfaces;

/// <summary>
/// Service interface for product operations.
/// </summary>
public interface IProductService
{
    Task<PagedResultDto<ProductDto>> GetAllProductsAsync(int page = 1, int pageSize = 10, string? search = null, int? categoryId = null, CancellationToken ct = default);
    Task<ProductDto?> GetProductByIdAsync(int id, CancellationToken ct = default);
    Task<ProductDto> CreateProductAsync(CreateProductDto dto, CancellationToken ct = default);
    Task<ProductDto?> UpdateProductAsync(int id, UpdateProductDto dto, CancellationToken ct = default);
    Task<bool> DeleteProductAsync(int id, CancellationToken ct = default);
}
