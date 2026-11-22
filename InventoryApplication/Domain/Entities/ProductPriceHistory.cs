namespace InventoryApplication.Domain.Entities;

public class ProductPriceHistory
{
    public int Id { get; set; }
    public decimal SalePrice { get; set; }
    public DateTime ChangedAt { get; set; } =  DateTime.UtcNow;
    public string? Description { get; set; }
    public decimal PurchPrice { get; set; } 
    //رابطه با product
    public int ProductId { get; set; }
    public Product Product { get; set; }  

}