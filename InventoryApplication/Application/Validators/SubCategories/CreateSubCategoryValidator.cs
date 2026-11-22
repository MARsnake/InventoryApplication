using FluentValidation;
using InventoryApplication.Application.DTOs.SubCategories;

namespace InventoryApplication.Application.Validators.SubCategories;

public class CreateSubCategoryValidator : AbstractValidator<CreateSubCategoryDto>
{
    public CreateSubCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("نام زیردسته‌بندی الزامی است")
            .MaximumLength(150)
            .WithMessage("نام زیردسته‌بندی نباید بیشتر از 150 کاراکتر باشد");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("توضیحات نباید بیشتر از 1000 کاراکتر باشد");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("انتخاب دسته‌بندی الزامی است");
    }
}