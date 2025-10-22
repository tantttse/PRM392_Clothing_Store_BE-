using ClothingStore.Domain.Enums;
using Shared.Application.Abstractions.DTOs;
using Shared.Domain.Common.ResponseModel.Pagination;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ClothingStore.Application.Features.Products.Dtos;

public class ProductDto : BaseDto<Guid>
{
    public string ProductName { get; set; } = default!;
    public string? BriefDescription { get; set; }
    public string? FullDescription { get; set; }
    public string? TechnicalSpecifications { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public Guid CategoryId { get; set; }
    public string? CategoryName { get; set; }
}


public class ProductCreateDto
{
    public string ProductName { get; set; } = default!;
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; } 
    public string? BriefDescription { get; set; }
    public string? FullDescription { get; set; }
    public string? TechnicalSpecifications { get; set; }
    public string? ImageUrl { get; set; }
}


public class ProductUpdateDto
{
    public string ProductName { get; set; } = default!;
    public decimal Price { get; set; }
    public Guid? CategoryId { get; set; }
    public string? BriefDescription { get; set; }
    public string? FullDescription { get; set; }
    public string? TechnicalSpecifications { get; set; }
    public string? ImageUrl { get; set; }

}

public class ProductDeleteResponseDto
{
    public bool IsDeleted { get; set; }
}

public class ProductFilterDto : PageFilterRequestDto
{
    public Guid? CategoryId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? SearchTerm { get; set; }
}