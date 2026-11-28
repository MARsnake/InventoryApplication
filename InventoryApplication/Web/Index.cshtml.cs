using FluentValidation;
using InventoryApplication.Application.DTOs.Categories;
using InventoryApplication.Application.DTOs.Products;
using InventoryApplication.Application.DTOs.SubCategories;
using InventoryApplication.Application.Exeptions;
using InventoryApplication.Application.Interfaces.Categories;
using InventoryApplication.Application.Interfaces.Products;
using InventoryApplication.Application.Interfaces.SubCategories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InventoryApplication.Pages;

public class IndexModel : PageModel
{
    private readonly ICategoryService _categoryService;
    private readonly ISubCategoryService _subCategoryService;
    private readonly IProductService _productService;

    public IndexModel(
        ICategoryService categoryService,
        ISubCategoryService subCategoryService,
        IProductService productService)
    {
        _categoryService = categoryService;
        _subCategoryService = subCategoryService;
        _productService = productService;
    }

    public List<CategoryDto> Categories { get; private set; } = [];
    public List<SubCategoryDto> SubCategories { get; private set; } = [];
    public List<ProductDto> Products { get; private set; } = [];
    public bool? IsActiveFilter { get; private set; }

    [TempData]
    public string? SuccessMessage { get; set; }

    public List<string> ErrorMessages { get; private set; } = [];

    public async Task OnGetAsync(bool? isActive = null)
    {
        IsActiveFilter = isActive;
        await LoadDataAsync(isActive);
    }

    // ==================== Category ====================

    public Task<IActionResult> OnPostCreateCategoryAsync(CreateCategoryDto dto) =>
        ExecuteAsync(() => _categoryService.CreateAsync(dto), "دسته‌بندی با موفقیت اضافه شد.");

    public Task<IActionResult> OnPostUpdateCategoryAsync(UpdateCategoryDto dto) =>
        ExecuteAsync(() => _categoryService.UpdateAsync(dto), "دسته‌بندی بروزرسانی شد.");

    public Task<IActionResult> OnPostDeleteCategoryAsync(int id) =>
        ExecuteAsync(() => _categoryService.DeleteAsync(id), "دسته‌بندی حذف شد.");

    // ==================== SubCategory ====================

    public Task<IActionResult> OnPostCreateSubCategoryAsync(CreateSubCategoryDto dto) =>
        ExecuteAsync(() => _subCategoryService.CreateAsync(dto), "زیردسته‌بندی با موفقیت اضافه شد.");

    public Task<IActionResult> OnPostUpdateSubCategoryAsync(UpdateSubCategoryDto dto) =>
        ExecuteAsync(() => _subCategoryService.UpdateAsync(dto), "زیردسته‌بندی بروزرسانی شد.");

    public Task<IActionResult> OnPostDeleteSubCategoryAsync(int id) =>
        ExecuteAsync(() => _subCategoryService.DeleteAsync(id), "زیردسته‌بندی حذف شد.");

    // ==================== Product ====================

    public Task<IActionResult> OnPostCreateProductAsync(CreateProductDto dto) =>
        ExecuteAsync(() => _productService.CreateAsync(dto), "کالا با موفقیت ثبت شد.");

    public Task<IActionResult> OnPostUpdateProductAsync(UpdateProductDto dto) =>
        ExecuteAsync(() => _productService.UpdateAsync(dto), "اطلاعات کالا بروزرسانی شد.");

    public Task<IActionResult> OnPostToggleProductAsync(int id, bool activate) =>
        ExecuteAsync(
            () => activate ? _productService.ActivateAsync(id) : _productService.DeactivateAsync(id),
            activate ? "کالا فعال شد." : "کالا غیرفعال شد.");

    public Task<IActionResult> OnPostDeleteProductAsync(int id) =>
        ExecuteAsync(() => _productService.DeleteAsync(id), "کالا حذف شد.");

    // ==================== Helpers ====================

    private async Task<IActionResult> ExecuteAsync(Func<Task> action, string successMessage)
    {
        try
        {
            await action();
            SuccessMessage = successMessage;
            return RedirectToPage();
        }
        catch (ValidationException ex)
        {
            ErrorMessages = ex.Errors.Select(e => e.ErrorMessage).Distinct().ToList();
        }
        catch (BusinessRuleException ex)
        {
            ErrorMessages.Add(ex.Message);
        }
        catch (NotFoundException ex)
        {
            ErrorMessages.Add(ex.Message);
        }

        await LoadDataAsync();
        return Page();
    }

    private async Task LoadDataAsync(bool? isActive = null)
    {
        Categories = await _categoryService.GetAllAsync();
        SubCategories = await _subCategoryService.GetAllAsync();
        Products = await _productService.GetAllAsync(isActive);
    }
}
