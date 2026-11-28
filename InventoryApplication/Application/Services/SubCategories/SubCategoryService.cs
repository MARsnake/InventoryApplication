using FluentValidation;
using InventoryApplication.Application.DTOs.Categories;
using InventoryApplication.Application.DTOs.SubCategories;
using InventoryApplication.Application.Exeptions;
using InventoryApplication.Application.Interfaces.SubCategories;
using InventoryApplication.Domain.Entities;
using InventoryApplication.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryApplication.Application.Services.SubCategories;

public class SubCategoryService : ISubCategoryService
{
    private const string EntityName = "زیردسته‌بندی";
    private const string CategoryEntityName = "دسته‌بندی";

    private readonly AppDbContext _context;
    private readonly IValidator<CreateSubCategoryDto> _createValidator;
    private readonly IValidator<UpdateSubCategoryDto> _updateValidator;

    public SubCategoryService(
        AppDbContext context,
        IValidator<CreateSubCategoryDto> createValidator,
        IValidator<UpdateSubCategoryDto> updateValidator)
    {
        _context = context;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<SubCategoryDto> CreateAsync(CreateSubCategoryDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        await _createValidator.ValidateAndThrowAsync(dto);

        await EnsureCategoryExistsAsync(dto.CategoryId);

        var name = dto.Name.Trim();
        await EnsureNameIsUniqueInCategoryAsync(name, dto.CategoryId);

        var subCategory = new SubCategory
        {
            Name = name,
            Description = NormalizeDescription(dto.Description),
            CategoryId = dto.CategoryId
        };

        _context.SubCategories.Add(subCategory);
        await SaveChangesAsync();

        return (await GetByIdAsync(subCategory.Id))!;
    }

    public async Task<List<SubCategoryDto>> GetAllAsync(int? categoryId = null)
    {
        var query = _context.SubCategories.AsNoTracking();

        if (categoryId.HasValue)
        {
            query = query.Where(s => s.CategoryId == categoryId.Value);
        }

        return await query
            .OrderBy(s => s.Category.Name)
            .ThenBy(s => s.Name)
            .Select(s => new SubCategoryDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Category = new CategorySummaryDto
                {
                    Id = s.Category.Id,
                    Name = s.Category.Name
                }
            })
            .ToListAsync();
    }

    public async Task<SubCategoryDto?> GetByIdAsync(int subCategoryId)
    {
        return await _context.SubCategories
            .AsNoTracking()
            .Where(s => s.Id == subCategoryId)
            .Select(s => new SubCategoryDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Category = new CategorySummaryDto
                {
                    Id = s.Category.Id,
                    Name = s.Category.Name
                }
            })
            .FirstOrDefaultAsync();
    }

    public async Task<SubCategoryDto> UpdateAsync(UpdateSubCategoryDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        await _updateValidator.ValidateAndThrowAsync(dto);

        var subCategory = await _context.SubCategories.FirstOrDefaultAsync(s => s.Id == dto.Id)
                          ?? throw new NotFoundException(EntityName, dto.Id);

        if (subCategory.CategoryId != dto.CategoryId)
        {
            await EnsureCategoryExistsAsync(dto.CategoryId);
        }

        var name = dto.Name.Trim();
        await EnsureNameIsUniqueInCategoryAsync(name, dto.CategoryId, excludeId: subCategory.Id);

        subCategory.Name = name;
        subCategory.Description = NormalizeDescription(dto.Description);
        subCategory.CategoryId = dto.CategoryId;

        await SaveChangesAsync();

        return (await GetByIdAsync(subCategory.Id))!;
    }

    public async Task DeleteAsync(int subCategoryId)
    {
        var subCategory = await _context.SubCategories.FirstOrDefaultAsync(s => s.Id == subCategoryId)
                          ?? throw new NotFoundException(EntityName, subCategoryId);

        var hasProducts = await _context.Products.AnyAsync(p => p.SubCategoryId == subCategoryId);
        if (hasProducts)
        {
            throw new BusinessRuleException(
                "این زیردسته‌بندی دارای کالا است و قابل حذف نیست. ابتدا کالاهای آن را حذف یا به زیردسته‌بندی دیگری منتقل کنید");
        }

        _context.SubCategories.Remove(subCategory);
        await SaveChangesAsync();
    }

    // ---------------- Helpers ----------------

    private async Task EnsureCategoryExistsAsync(int categoryId)
    {
        var exists = await _context.Categories.AnyAsync(c => c.Id == categoryId);
        if (!exists)
        {
            throw new NotFoundException(CategoryEntityName, categoryId);
        }
    }

    private async Task EnsureNameIsUniqueInCategoryAsync(string name, int categoryId, int? excludeId = null)
    {
        var normalized = name.ToLower();

        var exists = await _context.SubCategories.AnyAsync(s =>
            s.CategoryId == categoryId &&
            s.Name.ToLower() == normalized &&
            (excludeId == null || s.Id != excludeId));

        if (exists)
        {
            throw new BusinessRuleException($"زیردسته‌بندی با نام «{name}» در این دسته‌بندی از قبل وجود دارد");
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
            throw new BusinessRuleException("زیردسته‌بندی با این نام در این دسته‌بندی از قبل وجود دارد");
        }
        catch (DbUpdateException ex) when (ex.IsForeignKeyViolation())
        {
            throw new BusinessRuleException(
                "عملیات به دلیل وابستگی داده‌ها انجام نشد (دسته‌بندی وجود ندارد یا زیردسته‌بندی دارای کالا است)");
        }
    }

    private static string? NormalizeDescription(string? description) =>
        string.IsNullOrWhiteSpace(description) ? null : description.Trim();
}
