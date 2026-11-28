using FluentValidation;
using InventoryApplication.Application.DTOs.Categories;
using InventoryApplication.Application.DTOs.SubCategories;
using InventoryApplication.Application.Exeptions;
using InventoryApplication.Application.Interfaces.Categories;
using InventoryApplication.Domain.Entities;
using InventoryApplication.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryApplication.Application.Services.Categories;

public class CategoryService : ICategoryService
{
    private const string EntityName = "دسته‌بندی";

    private readonly AppDbContext _context;
    private readonly IValidator<CreateCategoryDto> _createValidator;
    private readonly IValidator<UpdateCategoryDto> _updateValidator;

    public CategoryService(
        AppDbContext context,
        IValidator<CreateCategoryDto> createValidator,
        IValidator<UpdateCategoryDto> updateValidator)
    {
        _context = context;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        await _createValidator.ValidateAndThrowAsync(dto);

        var name = dto.Name.Trim();
        await EnsureNameIsUniqueAsync(name);

        var category = new Category
        {
            Name = name,
            Description = NormalizeDescription(dto.Description)
        };

        _context.Categories.Add(category);
        await SaveChangesAsync();

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
    }

    public async Task<List<CategoryDto>> GetAllAsync()
    {
        return await _context.Categories
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                SubCategories = c.SubCategories
                    .OrderBy(s => s.Name)
                    .Select(s => new SubCategorySummaryDto
                    {
                        Id = s.Id,
                        Name = s.Name
                    })
                    .ToList()
            })
            .ToListAsync();
    }

    public async Task<CategoryDto?> GetByIdAsync(int categoryId)
    {
        return await _context.Categories
            .AsNoTracking()
            .Where(c => c.Id == categoryId)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                SubCategories = c.SubCategories
                    .OrderBy(s => s.Name)
                    .Select(s => new SubCategorySummaryDto
                    {
                        Id = s.Id,
                        Name = s.Name
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<CategoryDto> UpdateAsync(UpdateCategoryDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        await _updateValidator.ValidateAndThrowAsync(dto);

        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == dto.Id)
                       ?? throw new NotFoundException(EntityName, dto.Id);

        var name = dto.Name.Trim();
        await EnsureNameIsUniqueAsync(name, excludeId: category.Id);

        category.Name = name;
        category.Description = NormalizeDescription(dto.Description);

        await SaveChangesAsync();

        return (await GetByIdAsync(category.Id))!;
    }

    public async Task DeleteAsync(int categoryId)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId)
                       ?? throw new NotFoundException(EntityName, categoryId);

        var hasSubCategories = await _context.SubCategories.AnyAsync(s => s.CategoryId == categoryId);
        if (hasSubCategories)
        {
            throw new BusinessRuleException(
                "این دسته‌بندی دارای زیردسته‌بندی است و قابل حذف نیست. ابتدا زیردسته‌بندی‌های آن را حذف یا منتقل کنید");
        }

        _context.Categories.Remove(category);
        await SaveChangesAsync();
    }

    // ---------------- Helpers ----------------

    private async Task EnsureNameIsUniqueAsync(string name, int? excludeId = null)
    {
        var normalized = name.ToLower();

        var exists = await _context.Categories.AnyAsync(c =>
            c.Name.ToLower() == normalized &&
            (excludeId == null || c.Id != excludeId));

        if (exists)
        {
            throw new BusinessRuleException($"دسته‌بندی با نام «{name}» از قبل وجود دارد");
        }
    }

    private async Task SaveChangesAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.IsUniqueViolation())
        {
            throw new BusinessRuleException("دسته‌بندی با این نام از قبل وجود دارد");
        }
        catch (DbUpdateException ex) when (ex.IsForeignKeyViolation())
        {
            throw new BusinessRuleException("این دسته‌بندی دارای زیردسته‌بندی است و قابل حذف نیست");
        }
    }

    private static string? NormalizeDescription(string? description) =>
        string.IsNullOrWhiteSpace(description) ? null : description.Trim();
}
