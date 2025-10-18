using AutoMapper;
using ClothingStore.Application.Features.Orders.Dtos;
using ClothingStore.Domain.Repositories;
using FluentValidation;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Common.ResponseModel;

// TODO: Replace string Status with enum OrderStatus
// enum OrderStatus
// {
//     Processing,
//     Paid,
//     Shipped,
//     Delivered,
//     Cancelled
// }

namespace ClothingStore.Application.Features.Orders.Commands.Handlers
{
    public class UpdateOrderStatusDtoValidator : AbstractValidator<UpdateOrderStatusDto>
    {
        public UpdateOrderStatusDtoValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("OrderId is required.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(status => new[] { "Processing", "Paid", "Shipped", "Delivered", "Cancelled" }
                    .Contains(status))
                .WithMessage("Invalid status. Allowed values: Processing, Paid, Shipped, Delivered, Cancelled.");
        }
    }
    public class UpdateOrderStatusCommandHandler : ICommandHandler<UpdateOrderStatusCommand, OrderDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public UpdateOrderStatusCommandHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<Result<OrderDto>> Handle(UpdateOrderStatusCommand command, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);
            if (order == null)
                return Result.Failure<OrderDto>(new Error("OrderNotFound", "Order does not exist."));

            // Update status
            switch (command.Status)
            {
                case "Paid": order.MarkAsPaid(); break;
                case "Shipped": order.MarkAsShipped(); break;
                case "Delivered": order.MarkAsDelivered(); break;
                case "Cancelled": order.Cancel(); break;
                default:
                    return Result.Failure<OrderDto>(new Error("InvalidStatus", "Invalid order status."));
            }

            _orderRepository.Update(order, cancellationToken);

            return Result.Success(_mapper.Map<OrderDto>(order));
        }
    }
}
