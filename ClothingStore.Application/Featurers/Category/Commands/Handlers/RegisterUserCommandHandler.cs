// using AutoMapper;
// using ClothingStore.Application.Features.User.Dtos;
// using ClothingStore.Domain.Entities;
// using ClothingStore.Domain.Repositories;
// using Shared.Application.Abstractions.Authentication;
// using Shared.Application.Abstractions.Messaging;
// using Shared.Domain.Common.ResponseModel;

// namespace ClothingStore.Application.Features.User.Commands.RegisterUser
// {
//     public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, UserResponseDto>
//     {
//         private readonly IUserRepository _userRepository;
//         private readonly IPasswordHasher _passwordHasher;
//         private readonly IMapper _mapper;

//         public RegisterUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IMapper mapper)
//         {
//             _userRepository = userRepository;
//             _passwordHasher = passwordHasher;
//             _mapper = mapper;
//         }

//         public async Task<Result<UserResponseDto>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
//         {
//             var existingUser = await _userRepository.GetUserByMailOrUserName(command.RegisterRequest.UserName, cancellationToken);
//             if (existingUser != null)
//             {
//                 return Result.Failure<UserResponseDto>(new Error("UserExist", "User already exists with the same email or username."));
//             }

//             var user = Users.Create(command.RegisterRequest.Email, command.RegisterRequest.UserName, _passwordHasher.HashPassword(command.RegisterRequest.Password));
//             await _userRepository.AddAsync(user, cancellationToken);
//             var dto = _mapper.Map<UserResponseDto>(user);
//             return Result.Success(dto);
//         }
//     }
// }
