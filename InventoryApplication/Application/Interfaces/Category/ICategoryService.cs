using InventoryApplication.Application.DTOs.Categories;

namespace InventoryApplication.Application.Interfaces.Categories;

public interface ICategoryService
{
    Task<CategoryDto> CreateAsync(CreateCategoryDto dto);

    Task<List<CategoryDto>> GetAllAsync();

    Task<CategoryDto?> GetByIdAsync(int categoryId);

    Task<CategoryDto> UpdateAsync(UpdateCategoryDto dto);

    Task DeleteAsync(int categoryId);
}