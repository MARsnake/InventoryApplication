using InventoryApplication.Application.DTOs.Categories;

namespace InventoryApplication.Application.DTOs.SubCategories;

public class SubCategoryDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }

    public CategorySummaryDto Category { get; set; }
}