using Shared.Domain.Common.DDD;

namespace ClothingStore.Domain.Entities
{
    public class Order : AggregateRoot<Guid>
    {
        public Guid CartId { get; private set; }
        public Guid UserId { get; private set; }
        public string PaymentMethod { get; private set; } = default!;
        public string BillingAddress { get; private set; } = default!;
        public string Status { get; private set; } = "Processing"; // Processing, Paid, Shipped, Delivered, Cancelled
        public DateTime OrderDate { get; private set; }
        public decimal TotalAmount { get; private set; }

        public virtual ICollection<OrderItem> Items { get; private set; } = new List<OrderItem>();

        private Order() { }

        public static Order Create(Guid userId, Guid cartId, string paymentMethod, string billingAddress, IEnumerable<CartItem> cartItems)
        {
            if (cartItems == null || !cartItems.Any())
                throw new InvalidOperationException("Cannot create order from empty cart.");

            var order = new Order
            {
                UserId = userId,
                CartId = cartId,
                PaymentMethod = paymentMethod,
                BillingAddress = billingAddress,
                OrderDate = DateTime.UtcNow,
                Status = "Processing"
            };

            foreach (var cartItem in cartItems)
            {
                order.Items.Add(new OrderItem(
                    cartItem.ProductId,
                    cartItem.Quantity,
                    cartItem.UnitPrice
                ));
            }

            order.TotalAmount = order.Items.Sum(i => i.SubTotal);
            return order;
        }

        public void MarkAsPaid() => Status = "Paid";
        public void MarkAsShipped() => Status = "Shipped";
        public void MarkAsDelivered() => Status = "Delivered";
        public void Cancel() => Status = "Cancelled";

        protected override void Apply(IDomainEvent @event)
        {
            // No domain events yet
        }
    }

}
