namespace ClothingStore.Application.Features.Payments.Dtos;

public class CreatePaymentDto
{
    public Guid CartId { get; set; }
    public decimal Amount { get; set; }
    public string ReturnUrl { get; set; } = default!;
    public string IpAddress { get; set; } = "127.0.0.1";
}

public class VnPayResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = default!;
    public string? TransactionNo { get; set; }
    public string? ResponseCode { get; set; }
}


public class VnPayResultDto
{
    public Guid CartId { get; set; }

    public bool PaymentSuccess { get; set; }
    public string PaymentMessage { get; set; } = default!;
    public string? TransactionNo { get; set; }
    public string? ResponseCode { get; set; }

    public string? PaymentUrl { get; set; }
    public DateTime? CheckedOutAt { get; set; }
}