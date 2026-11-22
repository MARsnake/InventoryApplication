namespace InventoryApplication.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public string? Description { get; set; }
        public decimal PurchPrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal Quantity { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get;private set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } =  DateTime.UtcNow;
        public DateTime? isDeactivated { get; set; } = DateTime.UtcNow;

        // Foreign Key به Category (هر محصول فقط یک Category)
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        // به ساب کتگوری 
        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }
        // تاریخچه قیمت
        public ICollection<ProductPriceHistory> ProductPriceHistories { get; set; } = new List<ProductPriceHistory>();
    }
}