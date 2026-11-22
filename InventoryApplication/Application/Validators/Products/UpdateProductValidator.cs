using FluentValidation;
using InventoryApplication.Application.DTOs.Products;

namespace InventoryApplication.Application.Validators.Products;

public class UpdateProductValidator : AbstractValidator<UpdateProductDto>
{
    public UpdateProductValidator()
    {
        RuleFor(p => p.Id)
            .GreaterThan(0)
            .WithMessage("شناسه محصول معتبر نیست");

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
    }
}