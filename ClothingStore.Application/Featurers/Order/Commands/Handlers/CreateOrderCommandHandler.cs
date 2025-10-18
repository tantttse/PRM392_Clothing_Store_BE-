using AutoMapper;
using ClothingStore.Application.Features.Orders.Dtos;
using ClothingStore.Domain.Entities;
using ClothingStore.Domain.Repositories;
using FluentValidation;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Common.ResponseModel;

namespace ClothingStore.Application.Features.Orders.Commands.Handlers
{

    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.CartId)
                .NotEmpty().WithMessage("CartId is required.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            // RuleFor(x => x.PaymentMethod)
            //     .NotEmpty().WithMessage("Payment method is required.")
            //     .MaximumLength(50);

            // RuleFor(x => x.BillingAddress)
            //     .NotEmpty().WithMessage("Billing address is required.")
            //     .MaximumLength(200);
        }
    }
    public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, OrderDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public CreateOrderCommandHandler(
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<Result<OrderDto>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            // Load the cart snapshot
            var cart = await _cartRepository.GetByIdAsync(command.CartId, cancellationToken);
            if (cart == null || !cart.Items.Any())
                return Result.Failure<OrderDto>(new Error("CartInvalid", "Cart not found or empty."));

            // Create the order aggregate from the cart
            var order = Order.Create(
                command.UserId,
                command.CartId,
                command.PaymentMethod,
                command.BillingAddress,
                cart.Items
            );

            await _orderRepository.AddAsync(order, cancellationToken);

            return Result.Success(_mapper.Map<OrderDto>(order));
        }
    }
}
