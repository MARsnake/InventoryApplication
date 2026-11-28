namespace InventoryApplication.Application.DTOs.Customers;

public class CreateCustomerDto
{
    public required string Name { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public int? CompanyId { get; set; }
}