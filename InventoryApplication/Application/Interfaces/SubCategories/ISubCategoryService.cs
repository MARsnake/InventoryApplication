using InventoryApplication.Application.DTOs.SubCategories;

namespace InventoryApplication.Application.Interfaces.SubCategories;

public interface ISubCategoryService
{
    Task<SubCategoryDto> CreateAsync(CreateSubCategoryDto dto);

    Task<List<SubCategoryDto>> GetAllAsync(int? categoryId = null);

    Task<SubCategoryDto?> GetByIdAsync(int subCategoryId);

    Task<SubCategoryDto> UpdateAsync(UpdateSubCategoryDto dto);

    Task DeleteAsync(int subCategoryId);
}