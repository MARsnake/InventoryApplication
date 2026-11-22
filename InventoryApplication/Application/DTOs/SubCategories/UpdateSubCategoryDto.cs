namespace InventoryApplication.Application.DTOs.SubCategories;

public class UpdateSubCategoryDto
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public int CategoryId { get; set; }
}