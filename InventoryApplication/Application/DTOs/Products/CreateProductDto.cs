namespace InventoryApplication.Application.DTOs.Products;

public class CreateProductDto
{
    public required string ProductName { get; set; }
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public int SubCategoryId { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal SalePrice { get; set; }
    public decimal Quantity { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
}