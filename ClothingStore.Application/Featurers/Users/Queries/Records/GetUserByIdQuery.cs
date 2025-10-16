using ClothingStore.Application.Features.User.Dtos;
using MediatR;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Common.ResponseModel;

namespace ClothingStore.Application.Features.User.Queries
{
public record GetUserByIdQuery(Guid Id) : IQuery<UserResponseDto>;
}
