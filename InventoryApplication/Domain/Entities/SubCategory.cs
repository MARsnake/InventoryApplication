namespace InventoryApplication.Domain.Entities;

public class SubCategory
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }

    // Foreign Key
    public int CategoryId { get; set; }
    public Category Category { get; set; }

    // Navigation Property
    public ICollection<Product> Products { get; set; } = new List<Product>();
}