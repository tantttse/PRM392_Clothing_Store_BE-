using ClothingStore.Application.Features.Products.Dtos;
using ClothingStore.Application.Features.User.Dtos;
using ClothingStore.Domain.Enums;
using Shared.Application.Abstractions.DTOs;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ClothingStore.Application.Features.Category.Dtos;

public class CategoryDto : BaseDto<Guid>
{
    public string CategoryName { get; set; } = default!;
    public List<ProductDto>? Products { get; set; }
}

public class CreateCategoryRequest
{
    public string CategoryName { get; set; } = default!;
}

// DTO for updating a category (API input)
public class UpdateCategoryRequest
{
    public Guid Id { get; set; } = default!;
    public string CategoryName { get; set; } = default!;
}

public class ProductLightWeightDto
{
    public Guid Id { get; set; } = default!;
    public string ProductName { get; set; } = default!;
    public decimal Price { get; set; } = default!;
}