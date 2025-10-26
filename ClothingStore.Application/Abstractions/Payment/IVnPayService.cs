// using Application.Features.VnPay.DTOs;

using ClothingStore.Application.Features.Carts.Dtos;
using ClothingStore.Application.Features.Payments.Dtos;

namespace Application.Abstractions.Payment
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(CreatePaymentDto dto);
        VnPayResponseDto ValidateResponse(IDictionary<string, string> queryParams);
    }
}
