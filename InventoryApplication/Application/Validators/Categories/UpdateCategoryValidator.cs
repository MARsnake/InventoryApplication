using FluentValidation;
using InventoryApplication.Application.DTOs.Categories;

namespace InventoryApplication.Application.Validators.Categories;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDto>
{
    public UpdateCategoryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("شناسه دسته‌بندی معتبر نیست");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("نام دسته‌بندی الزامی است")
            .MaximumLength(150)
            .WithMessage("نام دسته‌بندی نباید بیشتر از 150 کاراکتر باشد");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("توضیحات نباید بیشتر از 1000 کاراکتر باشد");
    }
}