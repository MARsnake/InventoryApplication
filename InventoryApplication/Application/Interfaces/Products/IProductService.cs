using InventoryApplication.Application.DTOs.Products;

namespace InventoryApplication.Application.Interfaces.Products;

public interface IProductService
{
    Task<ProductDto> CreateAsync(CreateProductDto dto);

    Task<List<ProductDto>> GetAllAsync(bool? isActive = null);

    Task<ProductDto?> GetByIdAsync(int productId);

    Task<ProductDto> UpdateAsync(UpdateProductDto dto);

    Task DeactivateAsync(int productId);

    Task ActivateAsync(int productId);

    Task DeleteAsync(int productId);
}