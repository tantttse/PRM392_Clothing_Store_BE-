using AutoMapper;
using ClothingStore.Domain.Entities;
using ClothingStore.Application.Features.User.Dtos;
using ClothingStore.Domain.Enums;
namespace ClothingStore.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Users, UserResponseDto>()
                .ForMember(dest => dest.Roles,
                        opt => opt.MapFrom(src => src.Roles.Select(r => r.ToString()).ToList()));

            CreateMap<RegisterRequestDto, Users>()
                .ConstructUsing(src => Users.Create(
                    src.Email,
                    src.UserName!,
                    string.Empty,
                    src.FirstName,
                    src.LastName,
                    src.PhoneNumber,
                    src.Address,
                    null,                        // profileImageUrl
                    null                            // role
                ))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        }
    }
}
