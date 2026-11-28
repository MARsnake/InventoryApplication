using System.Linq.Expressions;
using FluentValidation;
using InventoryApplication.Application.DTOs.Products;
using InventoryApplication.Application.Exeptions;
using InventoryApplication.Application.Interfaces.Products;
using InventoryApplication.Domain.Entities;
using InventoryApplication.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryApplication.Application.Services.Products;

public class ProductService : IProductService
{
    private const string EntityName = "کالا";
    private const string SubCategoryEntityName = "زیردسته‌بندی";
    private const int MaxSkuGenerationAttempts = 10;

    private readonly AppDbContext _context;
    private readonly IValidator<CreateProductDto> _createValidator;
    private readonly IValidator<UpdateProductDto> _updateValidator;

    public ProductService(
        AppDbContext context,
        IValidator<CreateProductDto> createValidator,
        IValidator<UpdateProductDto> updateValidator)
    {
        _context = context;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    // Projection مشترک Entity -> DTO (در SQL ترجمه می‌شود و نیازی به Include نیست)
    private static readonly Expression<Func<Product, ProductDto>> ToProductDto = p => new ProductDto
    {
        Id = p.Id,
        Name = p.Name,
        Sku = p.Sku,
        Description = p.Description,
        PurchPrice = p.PurchPrice,
        SalePrice = p.SalePrice,
        Quantity = p.Quantity,
        Unit = p.Unit,
        IsActive = p.IsActive,
        CreatedAt = p.CreatedAt,
        UpdatedAt = p.UpdatedAt,
        SubCategoryId = p.SubCategoryId,
        SubCategoryName = p.SubCategory.Name,
        CategoryId = p.SubCategory.CategoryId,
        CategoryName = p.SubCategory.Category.Name
    };

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        await _createValidator.ValidateAndThrowAsync(dto);

        await EnsureSubCategoryExistsAsync(dto.SubCategoryId);

        var now = DateTime.UtcNow;
        var sku = await GenerateUniqueSkuAsync();

        var product = new Product
        {
            Name = dto.ProductName.Trim(),
            Sku = sku,
            Description = NormalizeDescription(dto.Description),
            PurchPrice = dto.PurchasePrice,
            SalePrice = dto.SalePrice,
            Quantity = dto.Quantity,
            Unit = dto.Unit,
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = null,
            SubCategoryId = dto.SubCategoryId
        };

        // ثبت قیمت اولیه در تاریخچه قیمت
        product.ProductPriceHistories.Add(new ProductPriceHistory
        {
            PurchPrice = dto.PurchasePrice,
            SalePrice = dto.SalePrice,
            ChangedAt = now,
            Description = "قیمت اولیه هنگام ثبت کالا"
        });

        _context.Products.Add(product);
        await SaveChangesAsync();

        return (await GetByIdAsync(product.Id))!;
    }

    public async Task<List<ProductDto>> GetAllAsync(bool? isActive = null)
    {
        var query = _context.Products.AsNoTracking();

        if (isActive.HasValue)
        {
            query = query.Where(p => p.IsActive == isActive.Value);
        }

        return await query
            .OrderBy(p => p.Name)
            .Select(ToProductDto)
            .ToListAsync();
    }

    public async Task<ProductDto?> GetByIdAsync(int productId)
    {
        return await _context.Products
            .AsNoTracking()
            .Where(p => p.Id == productId)
            .Select(ToProductDto)
            .FirstOrDefaultAsync();
    }

    public async Task<ProductDto> UpdateAsync(UpdateProductDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        await _updateValidator.ValidateAndThrowAsync(dto);

        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == dto.Id)
                      ?? throw new NotFoundException(EntityName, dto.Id);

        if (product.SubCategoryId != dto.SubCategoryId)
        {
            await EnsureSubCategoryExistsAsync(dto.SubCategoryId);
        }

        var now = DateTime.UtcNow;

        // در صورت تغییر قیمت، یک رکورد در تاریخچه قیمت ثبت می‌شود
        var priceChanged = product.PurchPrice != dto.PurchasePrice ||
                           product.SalePrice != dto.SalePrice;

        if (priceChanged)
        {
            _context.ProductPriceHistories.Add(new ProductPriceHistory
            {
                ProductId = product.Id,
                PurchPrice = dto.PurchasePrice,
                SalePrice = dto.SalePrice,
                ChangedAt = now,
                Description =
                    $"تغییر قیمت - خرید: {product.PurchPrice} ← {dto.PurchasePrice} | فروش: {product.SalePrice} ← {dto.SalePrice}"
            });
        }

        product.Name = dto.ProductName.Trim();
        product.Description = NormalizeDescription(dto.Description);
        product.PurchPrice = dto.PurchasePrice;
        product.SalePrice = dto.SalePrice;
        product.SubCategoryId = dto.SubCategoryId;
        product.Unit = dto.Unit;
        product.UpdatedAt = now;
        // نکته: موجودی (Quantity) عمداً از طریق ویرایش کالا تغییر نمی‌کند؛
        // تغییر موجودی باید از طریق ورود/خروج انبار (StockMovement) انجام شود.

        await SaveChangesAsync();

        return (await GetByIdAsync(product.Id))!;
    }

    public async Task DeactivateAsync(int productId)
    {
        await SetActiveStateAsync(productId, isActive: false);
    }

    public async Task ActivateAsync(int productId)
    {
        await SetActiveStateAsync(productId, isActive: true);
    }

    public async Task DeleteAsync(int productId)
    {
        var product = await _context.Products
                          .Include(p => p.ProductPriceHistories)
                          .FirstOrDefaultAsync(p => p.Id == productId)
                      ?? throw new NotFoundException(EntityName, productId);

        // رابطه تاریخچه قیمت Restrict است، پس ابتدا تاریخچه حذف می‌شود.
        // هر دو حذف در یک SaveChanges (یک تراکنش) انجام می‌شوند.
        _context.ProductPriceHistories.RemoveRange(product.ProductPriceHistories);
        _context.Products.Remove(product);

        await SaveChangesAsync();
    }

    // ---------------- Helpers ----------------

    private async Task SetActiveStateAsync(int productId, bool isActive)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId)
                      ?? throw new NotFoundException(EntityName, productId);

        if (product.IsActive == isActive)
        {
            return;
        }

        product.IsActive = isActive;
        product.UpdatedAt = DateTime.UtcNow;

        await SaveChangesAsync();
    }

    private async Task EnsureSubCategoryExistsAsync(int subCategoryId)
    {
        var exists = await _context.SubCategories.AnyAsync(s => s.Id == subCategoryId);
        if (!exists)
        {
            throw new NotFoundException(SubCategoryEntityName, subCategoryId);
        }
    }

    /// <summary>
    /// تولید SKU یکتا با فرمت PRD-yyMMdd-XXXXXXXX (حداکثر 50 کاراکتر طبق تنظیمات دیتابیس).
    /// </summary>
    private async Task<string> GenerateUniqueSkuAsync()
    {
        for (var attempt = 0; attempt < MaxSkuGenerationAttempts; attempt++)
        {
            var sku = $"PRD-{DateTime.UtcNow:yyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpperInvariant()}";

            var exists = await _context.Products.AnyAsync(p => p.Sku == sku);
            if (!exists)
            {
                return sku;
            }
        }

        throw new BusinessRuleException("تولید کد کالا (SKU) یکتا ممکن نشد. لطفاً دوباره تلاش کنید");
    }

    private async Task SaveChangesAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.IsUniqueViolation())
        {
            throw new BusinessRuleException("کد کالا (SKU) تکراری است. لطفاً دوباره تلاش کنید");
        }
        catch (DbUpdateException ex) when (ex.IsForeignKeyViolation())
        {
            throw new BusinessRuleException(
                "عملیات به دلیل وابستگی داده‌ها انجام نشد. اگر این کالا در اسناد دیگر استفاده شده، به جای حذف آن را غیرفعال کنید");
        }
    }

    private static string? NormalizeDescription(string? description) =>
        string.IsNullOrWhiteSpace(description) ? null : description.Trim();
}
