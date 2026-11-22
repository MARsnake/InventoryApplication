using FluentValidation;
using InventoryApplication.Application.DTOs.Categories;

namespace InventoryApplication.Application.Validators.Categories;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryDto>
{
    public CreateCategoryValidator()
    {
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