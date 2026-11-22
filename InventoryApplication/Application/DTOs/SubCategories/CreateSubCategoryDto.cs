namespace InventoryApplication.Application.DTOs.SubCategories;

public class CreateSubCategoryDto
{
    public required string Name { get; set; }

    public string? Description { get; set; }

    public int CategoryId { get; set; }
}