using Shared.Application.Abstractions.DTOs;
using Shared.Application.Abstractions.Messaging;

namespace ClothingStore.Application.Features.User.Commands.Login
{
    public record RefreshTokenCommand(RefreshTokenRequestDto Request) : ICommand<LoginResponseDto>;

}
