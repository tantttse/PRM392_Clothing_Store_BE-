using AutoMapper;
using ClothingStore.Application.Features.Carts.Dtos;
using ClothingStore.Domain.Repositories;
using FluentValidation;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Common.ResponseModel;

namespace ClothingStore.Application.Features.Carts.Commands;

public class RemoveCartItemCommandValidator : AbstractValidator<RemoveCartItemCommand>
    {
        public RemoveCartItemCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.Item)
                .NotNull().WithMessage("Cart item is required.");

            RuleFor(x => x.Item.ProductId)
                .NotEmpty().WithMessage("ProductId is required.");
        }
    }
public class RemoveCartItemCommandHandler : ICommandHandler<RemoveCartItemCommand, CartDto>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    public RemoveCartItemCommandHandler(
        ICartRepository cartRepository,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    public async Task<Result<CartDto>> Handle(RemoveCartItemCommand command, CancellationToken cancellationToken)
    {
        var userId = command.UserId;
        var productId = command.Item.ProductId;

        var cart = await _cartRepository.GetActiveCartByUserIdAsync(userId, cancellationToken);
        if (cart == null)
            return Result.Failure<CartDto>(new Error("CartNotFound", "No active cart found for user."));

        cart.RemoveItem(productId);

        if (!cart.Items.Any())
        {
            _cartRepository.Delete(cart, cancellationToken);
            return Result.Success<CartDto>(null!);
        }

        _cartRepository.Update(cart, cancellationToken);

        return Result.Success(_mapper.Map<CartDto>(cart));
    }
}
