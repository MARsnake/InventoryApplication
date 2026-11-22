using InventoryApplication.Domain.Enum;
namespace InventoryApplication.Application.DTOs.Products;

public class ProductDto
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Sku { get; set; }

    public string? Description { get; set; }

    public decimal PurchPrice { get; set; }

    public decimal SalePrice { get; set; }

    public decimal Quantity { get; set; }
    public UnitType Unit { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // SubCategory Information
    public int SubCategoryId { get; set; }

    public required string SubCategoryName { get; set; }

    // Category Information (Derived)
    public int CategoryId { get; set; }

    public required string CategoryName { get; set; }
}