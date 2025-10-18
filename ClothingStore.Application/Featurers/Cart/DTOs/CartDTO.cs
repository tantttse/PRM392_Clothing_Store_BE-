using Shared.Application.Abstractions.DTOs;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ClothingStore.Application.Features.Carts.Dtos;

public class CartDto : BaseDto<Guid>
{
    [JsonPropertyName("user_id")]
    public Guid UserId { get; set; }

    [JsonPropertyName("total_price")]
    [DefaultValue(0)]
    public decimal TotalPrice { get; set; }

    [JsonPropertyName("status")]
    [DefaultValue("Active")]
    public string Status { get; set; } = "Active";

    [JsonPropertyName("items")]
    [DefaultValue(typeof(List<CartItemDto>), "[]")]
    public List<CartItemDto> Items { get; set; } = new();
}

public class CartItemDto : BaseDto<Guid>
{
    [JsonPropertyName("product_id")]
    public Guid ProductId { get; set; }

    [JsonPropertyName("product_name")]
    [DefaultValue("")]
    public string ProductName { get; set; } = default!;

    [JsonPropertyName("unit_price")]
    [DefaultValue(0)]
    public decimal UnitPrice { get; set; }

    [JsonPropertyName("quantity")]
    [DefaultValue(1)]
    public int Quantity { get; set; }

    [JsonPropertyName("sub_total")]
    public decimal SubTotal => Quantity * UnitPrice;

    [JsonPropertyName("image_url")]
    [DefaultValue(null)]
    public string? ImageUrl { get; set; }
}

public class AddToCartDto
{
    [JsonPropertyName("product_id")]
    public Guid ProductId { get; set; }

    [JsonPropertyName("quantity")]
    [DefaultValue(1)]
    public int Quantity { get; set; }
}

public class UpdateCartItemDto
{
    [JsonPropertyName("product_id")]
    public Guid ProductId { get; set; }

    [JsonPropertyName("quantity")]
    [DefaultValue(1)]
    public int Quantity { get; set; }
}

public class RemoveCartItemDto
{
    [JsonPropertyName("product_id")]
    public Guid ProductId { get; set; }
}

public class CartCheckoutDto
{
    [JsonPropertyName("cart_id")]
    public Guid CartId { get; set; }
}
