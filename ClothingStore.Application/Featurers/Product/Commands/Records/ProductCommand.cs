using ClothingStore.Application.Features.Products.Dtos;
using ClothingStore.Application.Features.User.Dtos;
using Shared.Application.Abstractions.DTOs;
using Shared.Application.Abstractions.Messaging;

namespace ClothingStore.Application.Features.Products.Commands
{
    public record ProductCreateCommand(ProductCreateDto Product) : ICommand<ProductDto>;

    public record ProductUpdateCommand(Guid Id, ProductUpdateDto Product) : ICommand<ProductDto>;

    public record ProductDeleteCommand(Guid Id) : ICommand;
}
