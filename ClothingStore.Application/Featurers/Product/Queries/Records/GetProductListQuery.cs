using ClothingStore.Application.Features.Products.Dtos;
using ClothingStore.Application.Features.User.Dtos;
using MediatR;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Common.ResponseModel.Pagination;

namespace ClothingStore.Application.Features.Products.Queries
{
    public record GetProductListQuery(ProductFilterDto Filter) : IQuery<PaginatedResult<ProductDto>>;
}
