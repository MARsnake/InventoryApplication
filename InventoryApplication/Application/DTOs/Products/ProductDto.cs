


namespace InventoryApplication.Application.DTOs.Products;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Sku { get; set; }
    public string? Description { get; set; }
    public decimal PurchPrice { get; set; }
    public decimal SalePrice { get; set; }
    public decimal Quantity { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get;private set; }
        
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } =  DateTime.UtcNow;
    public DateTime? isDeactivated { get; set; } = DateTime.UtcNow;
    
}