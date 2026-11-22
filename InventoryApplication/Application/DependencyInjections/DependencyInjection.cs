using FluentValidation;
using InventoryApplication.Application.Validators.Products;

namespace InventoryApplication.Application.DependencyInjections;
//فقط چون creat رو تعریف میکنیم مابقی رو خودش پیدا میکنه. این به عنوان Marker عمل میکنه
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
    
        services.AddValidatorsFromAssemblyContaining<CreateProductValidator>();
        return services;
    }
    
}