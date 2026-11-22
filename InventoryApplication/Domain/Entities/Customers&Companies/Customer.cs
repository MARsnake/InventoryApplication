namespace InventoryApplication.Domain.Entities.Customers_Companies;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? CompanyName { get; set; }
    //fk به company
    public int? CompanyId { get; set; }
    public Company? Company { get; set; }
}