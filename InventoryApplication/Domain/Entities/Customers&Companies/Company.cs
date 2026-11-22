namespace InventoryApplication.Domain.Entities.Customers_Companies;

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    //مشخصات اقتصادی  
    public string? NationalId { get; set; }
    public string? EconomicCode { get; set; }
    public string? RegistrationNumber { get; set; }
    // کالشکن به خریدار های این کار 
    ICollection<Customer>  Customers { get; set; }
    
}