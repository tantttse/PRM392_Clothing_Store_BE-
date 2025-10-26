using ClothingStore.Application.Features.Orders.Dtos;
using Shared.Application.Abstractions.Messaging;

namespace ClothingStore.Application.Features.Orders.Commands
{
    // Optional: if you want to allow manual order creation
    public record CreateOrderCommand(Guid UserId,string PaymentMethod, string BillingAddress) 
        : ICommand<OrderDto>;

    // Update order status (Paid, Shipped, Delivered, Cancelled)
    public record UpdateOrderStatusCommand(Guid OrderId, string Status) 
        : ICommand<OrderDto>;

    // Cancel order (shortcut command)
    public record CancelOrderCommand(Guid OrderId) 
        : ICommand<OrderDto>;
}
