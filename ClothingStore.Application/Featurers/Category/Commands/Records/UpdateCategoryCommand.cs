using ClothingStore.Application.Features.Categories.Dtos;
using ClothingStore.Application.Features.User.Dtos;
using Shared.Application.Abstractions.DTOs;
using Shared.Application.Abstractions.Messaging;

namespace ClothingStore.Application.Features.Categories.Commands
{
    public record UpdateCategoryCommand(UpdateCategoryDto Category) : ICommand<CategoryDto>;

}
