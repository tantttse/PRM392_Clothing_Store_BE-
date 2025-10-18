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
            if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.");

            var existing = Items.FirstOrDefault(i => i.ProductId == product.Id);
            if (existing != null)
            {
                existing.UpdateQuantity(existing.Quantity + quantity);
            }
            else
            {
                var item = new CartItem(Id, product.Id, quantity, product.Price);
                Items.Add(item);
            }
            RecalculateTotal();
        }

        public void RemoveItem(Guid productId)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                Items.Remove(item);
                RecalculateTotal();
            }
        }

        public void UpdateItemQuantity(Guid productId, int newQuantity)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null) throw new InvalidOperationException("Item not found in cart.");
            item.UpdateQuantity(newQuantity);
            RecalculateTotal();
        }

        public void Checkout()
        {
            if (!Items.Any())
                throw new InvalidOperationException("Cannot checkout an empty cart.");

            Status = "Completed";

            // Raise domain event
            RaiseEvent(new CartCheckedOutEvent(Id, UserId, TotalPrice, DateTime.UtcNow));
        }


        private void RecalculateTotal()
        {
            TotalPrice = Items.Sum(i => i.SubTotal);
        }

        protected override void Apply(IDomainEvent @event) { }
    }
}
