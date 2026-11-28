namespace InventoryApplication.Application.DTOs.Companies;

public class CreateCompanyDto
{
    public required string Name { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Website { get; set; }

    public string? NationalId { get; set; }

    public string? EconomicCode { get; set; }

    public string? RegistrationNumber { get; set; }
}