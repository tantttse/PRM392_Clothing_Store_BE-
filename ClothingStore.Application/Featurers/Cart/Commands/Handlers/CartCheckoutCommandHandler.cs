using AutoMapper;
using ClothingStore.Application.Features.Carts.Dtos;
using ClothingStore.Domain.Repositories;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Common.ResponseModel;

namespace ClothingStore.Application.Features.Carts.Commands;

public class CartCheckoutCommandHandler : ICommandHandler<CartCheckoutCommand, CartDto>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    public CartCheckoutCommandHandler(ICartRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    public async Task<Result<CartDto>> Handle(CartCheckoutCommand command, CancellationToken cancellationToken)
    {
        var userId = command.UserId;
        var cart = await _cartRepository.GetActiveCartByUserIdAsync(userId, cancellationToken);

        if (cart == null || !cart.Items.Any())
            return Result.Failure<CartDto>(new Error("CartInvalid", "No active cart or cart is empty."));

        cart.Checkout(); // sets Status = "Completed"
        _cartRepository.Update(cart, cancellationToken);

        //  Optionally raise a domain event here: new CartCheckedOutEvent(cart.Id, userId)

        return Result.Success(_mapper.Map<CartDto>(cart));
    }
}
