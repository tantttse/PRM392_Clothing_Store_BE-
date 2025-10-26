using Application.Abstractions.Payment;
using AutoMapper;
using ClothingStore.Application.Features.Carts.Dtos;
using ClothingStore.Application.Features.Payments.Dtos;
using ClothingStore.Domain.Repositories;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Common.ResponseModel;

namespace ClothingStore.Application.Features.Carts.Commands;

public class CartCheckoutCommandHandler : ICommandHandler<CartCheckoutCommand, CartCheckoutResultDto>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IVnPayService _vnPayService;

        public CartCheckoutCommandHandler(ICartRepository cartRepository, IVnPayService vnPayService)
        {
            _cartRepository = cartRepository;
            _vnPayService = vnPayService;
        }

        public async Task<Result<CartCheckoutResultDto>> Handle(CartCheckoutCommand command, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetActiveCartByUserIdAsync(command.UserId, cancellationToken);
            if (cart == null || !cart.Items.Any())
                return Result.Failure<CartCheckoutResultDto>(new Error("Cart.NotFound", "Your cart is empty."));

            var paymentUrl = _vnPayService.CreatePaymentUrl(new CreatePaymentDto
            {
                CartId = cart.Id,
                Amount = cart.TotalPrice,
                ReturnUrl = "return url server"
            });

            return Result.Success(new CartCheckoutResultDto
            {
                CartId = cart.Id,
                PaymentSuccess = false,
                PaymentUrl = paymentUrl,
                PaymentMessage = "Redirect user to VNPay for payment."
            });
        }
    }