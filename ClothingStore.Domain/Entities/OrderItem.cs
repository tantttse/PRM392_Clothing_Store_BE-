using Shared.Domain.Common.DDD;
using System;

namespace ClothingStore.Domain.Entities
{
    /// <summary>
    /// Represents a single item within a customer's order.
    /// </summary>
    public class OrderItem : Entity<Guid>
    {
        // ====== Properties ======
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        /// <summary>
        /// Gets the total cost for this item (Quantity * UnitPrice).
        /// </summary>
        public decimal SubTotal => Quantity * UnitPrice;

        // ====== Constructors ======
        private OrderItem() { } // For EF Core

        internal OrderItem(Guid productId, int quantity, decimal unitPrice)
        {
            if (productId == Guid.Empty)
                throw new ArgumentException("Product ID cannot be empty.", nameof(productId));

            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");

            if (unitPrice <= 0)
                throw new ArgumentOutOfRangeException(nameof(unitPrice), "Unit price must be greater than zero.");

            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        // ====== Factory Method ======
        public static OrderItem Create(Guid productId, int quantity, decimal unitPrice)
        {
            return new OrderItem(productId, quantity, unitPrice);
        }

        // ====== Behavior Methods ======
        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(newQuantity), "Quantity must be greater than zero.");

            Quantity = newQuantity;
        }

        public void UpdateUnitPrice(decimal newPrice)
        {
            if (newPrice <= 0)
                throw new ArgumentOutOfRangeException(nameof(newPrice), "Price must be greater than zero.");

            UnitPrice = newPrice;
        }
    }
}
