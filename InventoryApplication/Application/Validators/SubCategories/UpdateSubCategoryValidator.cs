using FluentValidation;
using InventoryApplication.Application.DTOs.SubCategories;
namespace InventoryApplication.Application.Validators.SubCategories;

public class UpdateSubCategoryValidator : AbstractValidator<UpdateSubCategoryDto>
{
    public  UpdateSubCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("انتخاب دسته‌بندی الزامی است");
        RuleFor(p => p.Name).MaximumLength(150).WithMessage("نباید بیشتر از 150 کارکتر باشد");
        RuleFor(x => x.Description).MaximumLength(1000).WithMessage("توضیحات نباید بیشتر از 1000 کارکتر باشد");
    }
}