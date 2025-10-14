using ClothingStore.Application.Features.User.Dtos;
using Shared.Application.Abstractions.DTOs;
using Shared.Application.Abstractions.Messaging;

namespace ClothingStore.Application.Features.User.Commands.Login
{
    public record LoginUserCommand(string EmailOrUserName, string Password) : ICommand<LoginResponse>;
}
