using ClothingStore.Domain.Repositories;
using MediatR;
using Shared.Application.Abstractions.Authentication;
using Shared.Application.Abstractions.DTOs;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Common.ResponseModel;

namespace ClothingStore.Application.Features.User.Commands.Login
{
    public class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, LoginResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;
        //private readonly IUnitOfWork _unitOfWork;

        public LoginUserCommandHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenService jwtTokenService
            //IUnitOfWork unitOfWork
            )
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
            //_unitOfWork = unitOfWork;
        }

        public async Task<Result<LoginResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByMailOrUserName(request.EmailOrUserName, cancellationToken);
            if (user == null)
            {
                return Result.Failure<LoginResponse>(new Error("UserNotFound", "user not found or ."));
            }

            if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                return Result.Failure<LoginResponse>(new Error("InvalidCredentials", "Invalid password."));
            }

            var roles = user.Roles.Select(r => r.ToString()).ToList();
            var token = _jwtTokenService.GenerateToken(user.Id, user.Email, user.UserName, roles);
            var response = new LoginResponse(
                AccessToken: token,
                RefreshToken: _jwtTokenService.GenerateRefreshToken(),
                ExpiresAt: DateTime.UtcNow.AddHours(10),
                User: new UserInfo(user.Id, user.FirstName ?? "", user.UserName, user.Email, roles)
            );
            return Result.Success(response);
        }
    }
}