using AutoMapper;
using ClothingStore.Application.Features.Orders.Dtos;
using ClothingStore.Domain.Repositories;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Common.ResponseModel;

namespace ClothingStore.Application.Features.Orders.Commands.Handlers
{
    public class CancelOrderCommandHandler : ICommandHandler<CancelOrderCommand, OrderDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public CancelOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<Result<OrderDto>> Handle(CancelOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);
            if (order == null)
                return Result.Failure<OrderDto>(new Error("OrderNotFound", "Order does not exist."));

            // Domain logic: cancel the order
            order.Cancel();

            _orderRepository.Update(order, cancellationToken);

            return Result.Success(_mapper.Map<OrderDto>(order));
        }
    }
}
