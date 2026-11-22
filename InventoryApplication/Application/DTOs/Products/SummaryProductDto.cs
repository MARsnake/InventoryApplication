namespace InventoryApplication.Application.DTOs.Products;

public class SummaryProductDto
{
    public int ProductId { get; set; }

    public required string ProductName { get; set; }

    public required string Sku { get; set; }
}