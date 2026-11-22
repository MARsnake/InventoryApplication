namespace InventoryApplication.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }

    // Navigation Property
    public ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
}