using ClothingStore.Application.Features.Carts.Dtos;
using ClothingStore.Application.Features.Payments.Dtos;
using Shared.Application.Abstractions.Messaging;

public record VnPayReturnCommand(
        Guid CartId,
        string TransactionNo,
        string ResponseCode
    ) : ICommand<VnPayResultDto>;