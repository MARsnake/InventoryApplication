namespace InventoryApplication.Application.DTOs.SubCategories;

using InventoryApplication.Application.DTOs.Categories;

public class SubCategoryDto
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public required CategorySummaryDto Category { get; set; }
}