using FluentValidation;
using InventoryApplication.Application.DTOs.Customers;
namespace InventoryApplication.Application.Validators.Customers;

public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerDto>
{
    public  UpdateCustomerValidator()
    {
        RuleFor(c => c.Name).NotEmpty().WithMessage("نام اجباریست").MaximumLength(200);
        RuleFor(c => c.PhoneNumber).MaximumLength(14).WithMessage("شماره تلفن بیش از 14 رقم مجاز نیست");
        RuleFor(a => a.Address).MaximumLength(500).WithMessage("ادرس بیش از 500 کارکتر مجاز نیست");
        RuleFor(p=>p.Email).MaximumLength(150).EmailAddress();
        
    }
}