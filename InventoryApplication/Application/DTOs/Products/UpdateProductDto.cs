namespace InventoryApplication.Application.DTOs.Products;

public class UpdateProductDto
{
    public int Id { get; set; }
    public required string ProductName { get; set; }
    public string? Description { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal SalePrice { get; set; }
    public int CategoryId { get; set; }
    public int SubCategoryId { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}