using ClothingStore.Application.Features.Categories.Dtos;
using ClothingStore.Application.Features.User.Dtos;
using MediatR;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Common.ResponseModel;

namespace ClothingStore.Application.Features.Categories.Queries
{
    public record GetCategoryListQuery() : IQuery<IEnumerable<CategoryDto>>;
}
