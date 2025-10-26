using ClothingStore.Domain.Events;
using Shared.Domain.Common.DDD;

namespace ClothingStore.Domain.Entities
{
    public class Cart : AggregateRoot<Guid>
    {
        public Guid UserId { get; private set; }
        public decimal TotalPrice { get; private set; }
        public string Status { get; private set; } = "Active";
        public virtual Users User { get; private set; } = default!;

        // Navigation
        public ICollection<CartItem> Items { get; private set; } = new List<CartItem>();

        private Cart() { }

        public static Cart Create(Guid userId)
        {
            return new Cart
            {
                UserId = userId,
                TotalPrice = 0,
                Status = "Active",
            };
        }

        public void AddItem(Product product, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            var existing = Items.FirstOrDefault(i => i.ProductId == product.Id);
            if (existing != null)
            {
                existing.UpdateQuantity(existing.Quantity + quantity);
                // existing.UpdateProductInfo(product.ProductName, product.ImageUrl, product.Price);

            }
            else
            {
                var item = new CartItem(Id, product.Id, quantity, product.Price ,product.ProductName,product.ImageUrl);
                Items.Add(item);
            }
            RecalculateTotal();
        }

        public void RemoveItem(Guid productId, int quantityToRemove = 1)
        {
            if (quantityToRemove <= 0)
                throw new ArgumentException("Quantity to remove must be greater than zero.");

            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
                return;

            if (item.Quantity <= quantityToRemove)
                Items.Remove(item);
            else
                item.UpdateQuantity(item.Quantity - quantityToRemove);

            RecalculateTotal();
        }

        // Update quantity by product ID (only if exists)
        public void UpdateItemQuantity(Guid productId, int newQuantity)
        {
            if (newQuantity < 0)
                throw new ArgumentException("Quantity must be greater or equal to zero.");

            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
                throw new InvalidOperationException("Item not found in cart.");

            if (newQuantity == 0)
                Items.Remove(item);
            else
                item.UpdateQuantity(newQuantity);
                

            RecalculateTotal();
        }

        // Update or add item using Product reference
        public void UpdateOrAddItem(Product product, int newQuantity)
        {
            if (newQuantity < 0)
                throw new ArgumentException("Quantity must be greater or equal to zero.");

            var existing = Items.FirstOrDefault(i => i.ProductId == product.Id);

            if (existing != null)
            {
                if (newQuantity == 0)
                    Items.Remove(existing);
                else
                {
                    existing.UpdateQuantity(newQuantity);
                    //existing.UpdateProductInfo(product.ProductName, product.ImageUrl, product.Price);
                }
                    

            }
            else
            {
                if (newQuantity > 0)
                {
                    var item = new CartItem(Id, product.Id, newQuantity, product.Price ,product.ProductName,product.ImageUrl);
                    Items.Add(item);
                }
            }

            RecalculateTotal();
        }

        public void Checkout()
        {
            if (!Items.Any())
                throw new InvalidOperationException("Cannot checkout an empty cart.");

            Status = "Completed";
            RaiseEvent(new CartCheckedOutEvent(Id, UserId, TotalPrice, DateTime.UtcNow));
        }

        private void RecalculateTotal()
        {
            TotalPrice = Items.Sum(i => i.SubTotal);
        }

        

        protected override void Apply(IDomainEvent @event) { }
    }
}
