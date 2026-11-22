using FluentValidation;
using InventoryApplication.Application.DTOs.Products;

namespace InventoryApplication.Application.Validators.Products;

public class CreateProductValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductValidator()
    {
        RuleFor(p => p.ProductName)
            .NotEmpty()
            .WithMessage("نام محصول اجباری است")
            .MaximumLength(200)
            .WithMessage("نام محصول حداکثر باید 200 کاراکتر باشد");

        RuleFor(p => p.Description)
            .MaximumLength(1000)
            .WithMessage("توضیحات حداکثر می‌تواند 1000 کاراکتر باشد");

        RuleFor(p => p.SubCategoryId)
            .GreaterThan(0)
            .WithMessage("انتخاب زیردسته‌بندی الزامی است");

        RuleFor(p => p.PurchasePrice)
            .GreaterThan(0)
            .WithMessage("قیمت خرید باید بزرگ‌تر از صفر باشد");

        RuleFor(p => p.SalePrice)
            .GreaterThan(0)
            .WithMessage("قیمت فروش باید بزرگ‌تر از صفر باشد");

        RuleFor(p => p.Quantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("موجودی نمی‌تواند منفی باشد");
    }
}