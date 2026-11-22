using InventoryApplication.Application.DTOs.SubCategories;

namespace InventoryApplication.Application.DTOs.Categories;

public class CategoryDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }

    public ICollection<SubCategorySummaryDto> SubCategories { get; set; }
        = new List<SubCategorySummaryDto>();
}