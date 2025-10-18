using ClothingStore.Domain.Events;
using ClothingStore.Domain.Entities;
using ClothingStore.Domain.Repositories;
using MediatR;

namespace ClothingStore.Application.Features.Orders.EventHandlers
{
    public class CartCheckedOutEventHandler : INotificationHandler<CartCheckedOutEvent>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;

        public CartCheckedOutEventHandler(IOrderRepository orderRepository, ICartRepository cartRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
        }

        public async Task Handle(CartCheckedOutEvent notification, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetByIdAsync(notification.CartId, cancellationToken);
            if (cart == null) return;

            var order = Order.Create(
                notification.UserId,
                notification.CartId,
                paymentMethod: "Unpaid",
                billingAddress: "TBD",
                cartItems: cart.Items
            );
            //await mediator.Publish(new OrderCreatedIntegrationEvent(order.Id, order.UserId), cancellationToken);// Example of raising an integration event of external party
            await _orderRepository.AddAsync(order, cancellationToken);
        }
    }
}
