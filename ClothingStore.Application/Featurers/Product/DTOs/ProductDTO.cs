using ClothingStore.Domain.Enums;
using Shared.Application.Abstractions.DTOs;
using Shared.Domain.Common.ResponseModel.Pagination;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ClothingStore.Application.Features.Products.Dtos;

public class ProductDto : BaseDto<Guid>
{
    [DefaultValue("Sample Product")]
    public string ProductName { get; set; } = default!;

    [DefaultValue("A short description of the product")]
    public string? BriefDescription { get; set; }

    [DefaultValue("A detailed description of the product with specifications")]
    public string? FullDescription { get; set; }

    [DefaultValue("CPU: i7, RAM: 16GB, SSD: 512GB")]
    public string? TechnicalSpecifications { get; set; }

    [DefaultValue(99.99)]
    public decimal Price { get; set; }

    [DefaultValue("https://example.com/images/sample.png")]
    public string? ImageUrl { get; set; }

    [DefaultValue("0bdc793f-95ab-47de-8754-c571087c2201")]
    public Guid CategoryId { get; set; }

    [DefaultValue("Electronics")]
    public string? CategoryName { get; set; }

    // [DefaultValue(ProductStatus.Active)]
    // [JsonConverter(typeof(JsonStringEnumConverter))]
    // public ProductStatus Status { get; set; } = ProductStatus.Active;
}

public class ProductCreateDto
{
    [DefaultValue("New Product")]
    public string ProductName { get; set; } = default!;

    [DefaultValue(49.99)]
    public decimal Price { get; set; }

    [DefaultValue("0bdc793f-95ab-47de-8754-c571087c2201")]
    public Guid CategoryId { get; set; }

    [DefaultValue("Short description here")]
    public string? BriefDescription { get; set; }

    [DefaultValue("Full description here")]
    public string? FullDescription { get; set; }

    [DefaultValue("Specs go here")]
    public string? TechnicalSpecifications { get; set; }

    [DefaultValue("https://example.com/images/default.png")]
    public string? ImageUrl { get; set; }

    // [DefaultValue(ProductStatus.Draft)]
    // [JsonConverter(typeof(JsonStringEnumConverter))]
    // public ProductStatus Status { get; set; } = ProductStatus.Draft;
}

public class ProductUpdateDto
{
    [DefaultValue("Updated Product Name")]
    public string ProductName { get; set; } = default!;

    [DefaultValue(59.99)]
    public decimal Price { get; set; }

    [DefaultValue("d290f1ee-6c54-4b01-90e6-d701748f0851")]
    public Guid? CategoryId { get; set; }

    [DefaultValue("Updated short description")]
    public string? BriefDescription { get; set; }

    [DefaultValue("Updated full description")]
    public string? FullDescription { get; set; }

    [DefaultValue("Updated specs")]
    public string? TechnicalSpecifications { get; set; }

    [DefaultValue("https://example.com/images/updated.png")]
    public string? ImageUrl { get; set; }

    // [DefaultValue(ProductStatus.Active)]
    // [JsonConverter(typeof(JsonStringEnumConverter))]
    // public ProductStatus Status { get; set; } = ProductStatus.Active;
}

public class ProductDeleteResponseDto
{
    [DefaultValue(true)]
    public bool IsDeleted { get; set; }
}

public class ProductFilterDto : PageFilterRequestDto
{
    [DefaultValue("d290f1ee-6c54-4b01-90e6-d701748f0851")]
    public Guid? CategoryId { get; set; }

    [DefaultValue(0)]
    public decimal? MinPrice { get; set; }

    [DefaultValue(1000)]
    public decimal? MaxPrice { get; set; }

    [DefaultValue("laptop")]
    public string? SearchTerm { get; set; }

    // [DefaultValue(ProductStatus.Active)]
    // [JsonConverter(typeof(JsonStringEnumConverter))]
    // public ProductStatus? Status { get; set; } = ProductStatus.Active;
}
