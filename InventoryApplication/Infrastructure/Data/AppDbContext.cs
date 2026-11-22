
using Microsoft.EntityFrameworkCore;
using InventoryApplication.Domain.Entities;

namespace InventoryApplication.Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }
    public DbSet<ProductPriceHistory> ProductPriceHistories { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Product
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(P => P.Sku).IsRequired().HasMaxLength(50);


            entity.HasIndex(p => p.Sku)
                .IsUnique();
                
                

            entity.Property(p => p.Description)
                .HasMaxLength(1000);

            entity.Property(p => p.PurchPrice)
                .HasPrecision(18, 2);

            entity.Property(p => p.SalePrice)
                .HasPrecision(18, 2);

            entity.Property(p => p.Quantity)
                .HasPrecision(18, 2);

            
            // Product -> SubCategory (Required)
            entity.HasOne(p => p.SubCategory)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SubCategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Product -> ProductPriceHistory
            entity.HasMany(p => p.ProductPriceHistories)
                .WithOne(h => h.Product)
                .HasForeignKey(h => h.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });


        // Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(c => c.Description)
                .HasMaxLength(1000);

            entity.HasIndex(c => c.Name)
                .IsUnique();
        });


        // SubCategory
        modelBuilder.Entity<SubCategory>(entity =>
        {
            entity.HasKey(s => s.Id);

            entity.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(s => s.Description)
                .HasMaxLength(1000);

            entity.HasOne(s => s.Category)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(s => s.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(s => new { s.CategoryId, s.Name })
                .IsUnique();
        });


        // ProductPriceHistory
        modelBuilder.Entity<ProductPriceHistory>(entity =>
        {
            entity.HasKey(h => h.Id);

            entity.Property(h => h.PurchPrice)
                .HasPrecision(18, 2);

            entity.Property(h => h.SalePrice)
                .HasPrecision(18, 2);

            entity.Property(h => h.Description)
                .HasMaxLength(1000);

            entity.HasOne(h => h.Product)
                .WithMany(p => p.ProductPriceHistories)
                .HasForeignKey(h => h.ProductId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
