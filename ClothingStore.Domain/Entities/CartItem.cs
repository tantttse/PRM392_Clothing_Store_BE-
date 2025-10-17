using Shared.Domain.Common.DDD;

namespace ClothingStore.Domain.Entities
{
    public class CartItem : Entity<Guid>
    {
        public Guid CartId { get; private set; }
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal SubTotal => Quantity * UnitPrice;

        // Navigation
        public Cart Cart { get; private set; } = default!;
        public Product Product { get; private set; } = default!;

        private CartItem() { }

        internal CartItem(Guid cartId, Guid productId, int quantity, decimal unitPrice)
        {
            CartId = cartId;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0) throw new ArgumentException("Quantity must be greater than zero.");
            Quantity = newQuantity;
        }
    }
}
