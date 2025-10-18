using AutoMapper;
using ClothingStore.Application.Features.Carts.Dtos;
using ClothingStore.Domain.Entities;
using ClothingStore.Domain.Repositories;
using FluentValidation;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Common.ResponseModel;

namespace ClothingStore.Application.Features.Carts.Commands;

public class AddToCartCommandValidator : AbstractValidator<AddToCartCommand>
    {
        public AddToCartCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.Item)
                .NotNull().WithMessage("Cart item is required.");

            RuleFor(x => x.Item.ProductId)
                .NotEmpty().WithMessage("ProductId is required.");

            RuleFor(x => x.Item.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }
    }
public class AddToCartCommandHandler : ICommandHandler<AddToCartCommand, CartDto>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public AddToCartCommandHandler(
        ICartRepository cartRepository,
        IProductRepository productRepository,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<Result<CartDto>> Handle(AddToCartCommand command, CancellationToken cancellationToken)
    {
        var userId = command.UserId; // You may need to inject this from context/session
        var productId = command.Item.ProductId;
        var quantity = command.Item.Quantity;

        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
        if (product == null)
            return Result.Failure<CartDto>(new Error("ProductNotFound", "Product does not exist."));

        if (!product.IsInStock(quantity))
            return Result.Failure<CartDto>(new Error("OutOfStock", "Not enough stock available."));

        var cart = await _cartRepository.GetActiveCartByUserIdAsync(userId, cancellationToken);
        if (cart == null || cart.Status != "Active")
        {
            cart = Cart.Create(userId);
            await _cartRepository.AddAsync(cart, cancellationToken);
        }

        cart.AddItem(product, quantity);
        _cartRepository.Update(cart, cancellationToken);

        return Result.Success(_mapper.Map<CartDto>(cart));
    }
}
