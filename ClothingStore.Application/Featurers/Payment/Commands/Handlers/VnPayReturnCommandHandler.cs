using Application.Abstractions.Payment;
using ClothingStore.Application.Features.Carts.Commands;
using ClothingStore.Application.Features.Carts.Dtos;
using ClothingStore.Application.Features.Payments.Dtos;
using ClothingStore.Domain.Repositories;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Common.ResponseModel;

namespace ClothingStore.Application.Features.Payments.Commands
{
    public class VnPayReturnCommandHandler : ICommandHandler<VnPayReturnCommand, VnPayResultDto>
    {
        private readonly IVnPayService _vnPayService;
        private readonly ICartRepository _cartRepository;

        public VnPayReturnCommandHandler(IVnPayService vnPayService, ICartRepository cartRepository)
        {
            _vnPayService = vnPayService;
            _cartRepository = cartRepository;
        }

        public async Task<Result<VnPayResultDto>> Handle(VnPayReturnCommand command, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetByIdAsync(command.CartId, cancellationToken);
            if (cart == null)
                return Result.Failure<VnPayResultDto>(new Error("Cart.NotFound", "Cart not found."));

            // Validate VNPay response
            if (command.ResponseCode == "00") // success
            {
                cart.Checkout(); // domain logic
                _cartRepository.Update(cart, cancellationToken);

                return Result.Success(new VnPayResultDto
                {
                    CartId = cart.Id,
                    PaymentSuccess = true,
                    PaymentMessage = "Payment successful!",
                    TransactionNo = command.TransactionNo,
                    ResponseCode = command.ResponseCode,
                    CheckedOutAt = DateTime.UtcNow
                });
            }

            return Result.Failure<VnPayResultDto>(new Error("VNPay.Failed", "Payment failed or was canceled."));
        }
    }
}
