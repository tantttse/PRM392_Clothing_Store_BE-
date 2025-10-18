using Shared.Application.Abstractions.DTOs;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ClothingStore.Application.Features.Orders.Dtos;

public class OrderDto : BaseDto<Guid>
{
    [JsonPropertyName("cart_id")]
    public Guid CartId { get; set; }

    [JsonPropertyName("user_id")]
    public Guid UserId { get; set; }

    [JsonPropertyName("payment_method")]
    [DefaultValue("")]
    public string PaymentMethod { get; set; } = string.Empty;

    [JsonPropertyName("billing_address")]
    [DefaultValue("")]
    public string BillingAddress { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    [DefaultValue("Processing")]
    public string Status { get; set; } = "Processing";

    [JsonPropertyName("order_date")]
    public DateTime OrderDate { get; set; }

    [JsonPropertyName("total_amount")]
    [DefaultValue(0)]
    public decimal TotalAmount { get; set; }

    [JsonPropertyName("items")]
    [DefaultValue(typeof(List<OrderItemDto>), "[]")]
    public List<OrderItemDto> Items { get; set; } = new();
}

public class OrderItemDto : BaseDto<Guid>
{
    [JsonPropertyName("order_id")]
    public Guid OrderId { get; set; }

    [JsonPropertyName("product_id")]
    public Guid ProductId { get; set; }

    [JsonPropertyName("quantity")]
    [DefaultValue(1)]
    public int Quantity { get; set; }

    [JsonPropertyName("unit_price")]
    [DefaultValue(0)]
    public decimal UnitPrice { get; set; }

    [JsonPropertyName("sub_total")]
    public decimal SubTotal => Quantity * UnitPrice;
}

public class CreateOrderDto
{
    [JsonPropertyName("cart_id")]
    public Guid CartId { get; set; }

    [JsonPropertyName("user_id")]
    public Guid UserId { get; set; }

    [JsonPropertyName("payment_method")]
    public string PaymentMethod { get; set; } = string.Empty;

    [JsonPropertyName("billing_address")]
    public string BillingAddress { get; set; } = string.Empty;
}

public class UpdateOrderStatusDto
{
    [JsonPropertyName("order_id")]
    public Guid OrderId { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty; // Paid, Shipped, Delivered, Cancelled
}

public class AddOrderItemDto
{
    [JsonPropertyName("product_id")]
    public Guid ProductId { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("unit_price")]
    public decimal UnitPrice { get; set; }
}
