using ClothingStore.Application.Features.Carts.Dtos;
using Shared.Application.Abstractions.Messaging;

namespace ClothingStore.Application.Features.Carts.Queries;

public record GetCartByUserIdQuery(Guid UserId) : IQuery<CartDto>;

public record GetCartItemByProductIdQuery(Guid UserId, Guid ProductId) : IQuery<CartItemDto>;
