namespace InventoryApplication.Application.DTOs.Categories;

public class UpdateCategoryDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        
    }
