using System.Data;
using FluentValidation;
using InventoryApplication.Application.DTOs.Customers;
namespace InventoryApplication.Application.Validators.Customers;

public class CreatCustomerValidator : AbstractValidator<CreateCustomerDto>
{
    public  CreatCustomerValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("نام الزامیست")
            .MaximumLength(200)
            .WithMessage(" حداکثر طول مجاز 50 کارکتر میباشد");
        RuleFor(c => c.PhoneNumber)
            .MaximumLength(14).WithMessage("شماره موبایل بیشتر 14 رقم مجاز نیست");
        RuleFor(c => c.Email)
            .MaximumLength(150).WithMessage("ایمیل بیشتر 150 کارکتر مجاز نیست");

    }
}