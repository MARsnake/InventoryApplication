using FluentValidation;
using InventoryApplication.Application.Interfaces.Categories;
using InventoryApplication.Application.Interfaces.Products;
using InventoryApplication.Application.Interfaces.SubCategories;
using InventoryApplication.Application.Services.Categories;
using InventoryApplication.Application.Services.Products;
using InventoryApplication.Application.Services.SubCategories;
using InventoryApplication.Application.Validators.Products;

namespace InventoryApplication.Application.DependencyInjections;
//فقط چون creat رو تعریف میکنیم مابقی رو خودش پیدا میکنه. این به عنوان Marker عمل میکنه
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
    
        services.AddValidatorsFromAssemblyContaining<CreateProductValidator>();

        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ISubCategoryService, SubCategoryService>();
        services.AddScoped<IProductService, ProductService>();

        return services;
    }
    
}