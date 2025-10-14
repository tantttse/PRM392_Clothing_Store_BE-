using AutoMapper;
using ClothingStore.Application.Features.User.Dtos;
using ClothingStore.Domain.Entities;
using ClothingStore.Domain.Repositories;
using MediatR;
using Shared.Application.Abstractions.Authentication;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Common.ResponseModel;

namespace ClothingStore.Application.Features.User.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, UserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        public RegisterUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<Result<UserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetUserByMailOrUserName(request.UserName, cancellationToken);
            if (existingUser != null)
            {
                return Result.Failure<UserResponse>(new Error("UserExist", "User already exists with the same email or username."));
            }

            var user = Users.Create(request.Email, request.UserName, _passwordHasher.HashPassword(request.Password));
            await _userRepository.AddAsync(user, cancellationToken);
            var dto = _mapper.Map<UserResponse>(user);
            return Result.Success(dto);
        }
    }
}
