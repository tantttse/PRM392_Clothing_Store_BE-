using System.Security.Claims;
using AutoMapper;
using ClothingStore.Application.Features.User.Commands.Login;
using ClothingStore.Application.Features.User.Dtos;
using ClothingStore.Domain.Entities;
using ClothingStore.Domain.Repositories;
using FluentValidation;
using Shared.Application.Abstractions.Authentication;
using Shared.Application.Abstractions.DTOs;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Common.ResponseModel;

namespace ClothingStore.Application.Features.User.Commands.RegisterUser
{

    public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, LoginResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public RefreshTokenCommandHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<Result<LoginResponseDto>> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;

            //  Allow expired access token for refresh
            var principal = _jwtTokenService.ValidateToken(request.AccessToken, allowExpired: true);
            if (principal == null)
            {
                return Result.Failure<LoginResponseDto>(new Error("InvalidAccessToken", "Access token is invalid."));
            }

            var userId = Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                return Result.Failure<LoginResponseDto>(new Error("UserNotFound", "User not found."));
            }

            // Validate refresh token structure and signature if using jwt as token
            // var refreshPrincipal = _jwtTokenService.ValidateToken(request.RefreshToken);
            // if (refreshPrincipal == null)
            // {
            //     return Result.Failure<LoginResponseDto>(new Error("InvalidRefreshToken", "Refresh token is malformed or invalid."));
            // }

            //  Check if refresh token matches and is still valid
            if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiry <= DateTime.UtcNow)
            {
                return Result.Failure<LoginResponseDto>(new Error("RefreshTokenExpired", "Refresh token is invalid or expired."));
            }

            var roles = user.Roles.Select(r => r.ToString()).ToList();
            var newAccessToken = _jwtTokenService.GenerateToken(user.Id, user.Email, user.UserName, roles);
            var newRefreshToken = _jwtTokenService.GenerateRefreshToken();
            var newExpiry = DateTime.UtcNow.AddDays(7);

            user.SetRefreshToken(newRefreshToken, newExpiry);
            _userRepository.Update(user, cancellationToken);

            var response = new LoginResponseDto(
                AccessToken: newAccessToken,
                RefreshToken: newRefreshToken,
                ExpiresAt: DateTime.UtcNow.AddMinutes(60),
                User: new UserInfoDto(user.Id, user.FirstName ?? "", user.UserName, user.Email, roles)
            );

            return Result.Success(response);
        }

    }

}
