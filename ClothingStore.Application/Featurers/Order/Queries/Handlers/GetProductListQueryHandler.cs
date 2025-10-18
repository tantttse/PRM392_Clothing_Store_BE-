using AutoMapper;
using ClothingStore.Application.Features.Orders.Dtos;
using ClothingStore.Domain.Repositories;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Common.ResponseModel;

namespace ClothingStore.Application.Features.Orders.Queries
{
    public class GetOrdersByUserQueryHandler 
        : IQueryHandler<GetOrdersByUserQuery, List<OrderDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetOrdersByUserQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<OrderDto>>> Handle(GetOrdersByUserQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetByUserIdAsync(request.UserId, cancellationToken);

            if (orders == null || !orders.Any())
                return Result.Failure<List<OrderDto>>(new Error("OrdersNotFound", "No orders found for this user."));

            return Result.Success(_mapper.Map<List<OrderDto>>(orders));
        }
    }
}
