using AutoMapper;
using ClothingStore.Application.Features.Carts.Dtos;
using ClothingStore.Domain.Entities;
using ClothingStore.Domain.Repositories;
using FluentValidation;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Common.ResponseModel;

namespace ClothingStore.Application.Features.Carts.Commands;

public class UpdateToCartCommandValidator : AbstractValidator<UpdateCartItemCommand>
    {
        public UpdateToCartCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.Item)
                .NotNull().WithMessage("Cart item is required.");

            RuleFor(x => x.Item.ProductId)
                .NotEmpty().WithMessage("ProductId is required.");

            RuleFor(x => x.Item.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Quantity must be greater or equal zero.");
        }
    }
public class UpdateToCartCommandHandler : ICommandHandler<UpdateCartItemCommand, CartDto>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public UpdateToCartCommandHandler(
        ICartRepository cartRepository,
        IProductRepository productRepository,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<Result<CartDto>> Handle(UpdateCartItemCommand command, CancellationToken cancellationToken)
    {
        // validate product
        var product = await _productRepository.GetByIdAsync(command.Item.ProductId, cancellationToken);
        if (product is null)
            return Result.Failure<CartDto>(new Error("ProductNotFound", "Product does not exist."));

        if (!product.IsInStock(command.Item.Quantity))
            return Result.Failure<CartDto>(new Error("OutOfStock", "Not enough stock available."));

        // get or create cart
        var cart = await _cartRepository.GetActiveCartByUserIdAsync(command.UserId, cancellationToken)
                   ?? Cart.Create(command.UserId);

        // add item
        cart.UpdateOrAddItem(product, command.Item.Quantity);

        if (cart.Id == Guid.Empty)
            await _cartRepository.AddAsync(cart, cancellationToken);
        else
            _cartRepository.Update(cart, cancellationToken);

        return Result.Success(_mapper.Map<CartDto>(cart));
    }
}
