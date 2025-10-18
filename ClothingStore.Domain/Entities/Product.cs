using Shared.Domain.Common.DDD;

namespace ClothingStore.Domain.Entities
{
    public class Product : AggregateRoot<Guid>
    {
        public string ProductName { get; private set; } = default!;
        public string? BriefDescription { get; private set; }
        public string? FullDescription { get; private set; }
        public string? TechnicalSpecifications { get; private set; }
        public decimal Price { get; private set; }
        public string? ImageUrl { get; private set; }
        public int StockQuantity { get; private set; }
        // Relationships
        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; } = default!;

        private Product() { }

        public static Product Create(string name, string? brief, string? full, string? specs, string? imageUrl, decimal price, Guid categoryId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name cannot be empty.");
            if (price <= 0)
                throw new ArgumentException("Price must be greater than zero.");

            return new Product
            {
                ProductName = name,
                BriefDescription = brief,
                FullDescription = full,
                TechnicalSpecifications = specs,
                ImageUrl = imageUrl,
                Price = price,
                CategoryId = categoryId,
            };
        }

        public void UpdateDetails(string name, string? brief, string? full, string? specs, string? imageUrl, decimal price, Guid? categoryId)
        {
            ProductName = name ?? ProductName;
            BriefDescription = brief ?? BriefDescription;
            FullDescription = full ?? FullDescription;
            TechnicalSpecifications = specs ?? TechnicalSpecifications;
            ImageUrl = imageUrl ?? ImageUrl;
            Price = price > 0 ? price : Price;
            CategoryId = categoryId ?? CategoryId;
        }

        public void ChangePrice(decimal newPrice)
        {
            if (newPrice <= 0) throw new ArgumentException("Price must be greater than zero.");
            Price = newPrice;
        }

        public void IncreaseStock(int amount)
        {
            if (amount <= 0) throw new ArgumentException("Amount must be positive.");
            StockQuantity += amount;
        }

        public void DecreaseStock(int amount)
        {
            if (amount <= 0) throw new ArgumentException("Amount must be positive.");
            if (StockQuantity < amount) throw new InvalidOperationException("Not enough stock.");
            StockQuantity -= amount;
        }

        public bool IsInStock(int requestedQuantity) => StockQuantity >= requestedQuantity;

        protected override void Apply(IDomainEvent @event)
        {
            // No domain events yet
        }
    }
}
