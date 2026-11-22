using FluentValidation;
using InventoryApplication.Application.DTOs.Categories;

namespace InventoryApplication.Application.Validators.Categories;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDto>
{
    public UpdateCategoryValidator() {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
            .NotNull().WithMessage("نام نباید خالی باشد");
        RuleFor(p => p.Name).MaximumLength(150).WithMessage("نباید بیشتر از 150 کارکتر باشد");
        RuleFor(x => x.Description).MaximumLength(1000).WithMessage("توضیحات نباید بیشتر از 1000 کارکتر باشد");
    }
}