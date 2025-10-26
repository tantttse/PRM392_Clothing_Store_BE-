using Shared.Domain.Common.DDD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClothingStore.Domain.Entities
{
    public class Order : AggregateRoot<Guid>
    {
        public Guid CartId { get; private set; }
        public Guid UserId { get; private set; }
        public string PaymentMethod { get; private set; } = default!;
        public string BillingAddress { get; private set; } = default!;
        public string Status { get; private set; } = OrderStatus.Processing;
        public DateTime OrderDate { get; private set; }
        public decimal TotalAmount { get; private set; }
        public string PaymentStatus { get; private set; } = PaymentStatuses.Pending;

        public virtual ICollection<OrderItem> Items { get; private set; } = new List<OrderItem>();

        private Order() { } // EF Core

        private Order(Guid userId, Guid cartId, string paymentMethod, string billingAddress)
        {
            UserId = userId;
            CartId = cartId;
            PaymentMethod = paymentMethod;
            BillingAddress = billingAddress;
            OrderDate = DateTime.UtcNow;
        }

        public static Order Create(Guid userId, Guid cartId, string paymentMethod, string billingAddress, IEnumerable<CartItem> cartItems)
        {
            if (cartItems == null || !cartItems.Any())
                throw new InvalidOperationException("Cannot create an order from an empty cart.");

            var order = new Order(userId, cartId, paymentMethod, billingAddress);

            foreach (var cartItem in cartItems)
            {
                order.Items.Add(OrderItem.Create(cartItem.ProductId, cartItem.Quantity, cartItem.UnitPrice));
            }

            order.TotalAmount = order.Items.Sum(i => i.SubTotal);

            return order;
        }

        public void MarkAsPaid()
        {
            if (PaymentStatus != PaymentStatuses.Pending)
                throw new InvalidOperationException("Order is already paid or failed.");

            PaymentStatus = PaymentStatuses.Paid;
            Status = OrderStatus.Paid;
        }

        public void MarkAsFailed()
        {
            PaymentStatus = PaymentStatuses.Failed;
        }

        public void MarkAsShipped()
        {
            if (Status != OrderStatus.Paid)
                throw new InvalidOperationException("Order must be paid before shipping.");

            Status = OrderStatus.Shipped;
        }

        public void MarkAsDelivered()
        {
            if (Status != OrderStatus.Shipped)
                throw new InvalidOperationException("Order must be shipped before being delivered.");

            Status = OrderStatus.Delivered;
        }

        public void Cancel()
        {
            if (Status == OrderStatus.Delivered)
                throw new InvalidOperationException("Delivered orders cannot be cancelled.");

            Status = OrderStatus.Cancelled;
        }

        protected override void Apply(IDomainEvent @event) { }

        public static class OrderStatus
        {
            public const string Processing = "Processing";
            public const string Paid = "Paid";
            public const string Shipped = "Shipped";
            public const string Delivered = "Delivered";
            public const string Cancelled = "Cancelled";
        }

        public static class PaymentStatuses
        {
            public const string Pending = "Pending";
            public const string Paid = "Paid";
            public const string Failed = "Failed";
        }
    }
}
