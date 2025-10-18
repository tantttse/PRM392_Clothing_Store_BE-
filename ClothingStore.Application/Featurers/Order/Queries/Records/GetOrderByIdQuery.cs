using ClothingStore.Application.Features.Orders.Dtos;
using Shared.Application.Abstractions.Messaging;

namespace ClothingStore.Application.Features.Orders.Queries
{
    public record GetOrderByIdQuery(Guid Id) : IQuery<OrderDto>;
}
