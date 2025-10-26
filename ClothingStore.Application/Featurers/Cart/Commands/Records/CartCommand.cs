using ClothingStore.Application.Features.Carts.Dtos;
using Shared.Application.Abstractions.Messaging;

namespace ClothingStore.Application.Features.Carts.Commands;

public record AddToCartCommand(Guid UserId, AddToCartDto Item) : ICommand<CartDto>;

public record UpdateCartItemCommand(Guid UserId, UpdateCartItemDto Item) : ICommand<CartDto>;

public record RemoveCartItemCommand(Guid UserId, RemoveCartItemDto Item) : ICommand<CartDto>;

// public record CartCheckoutCommand(Guid UserId) : ICommand<CartDto>;
public record CartCheckoutCommand(Guid UserId) : ICommand<CartCheckoutResultDto>;
