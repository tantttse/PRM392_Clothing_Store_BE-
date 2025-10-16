using AutoMapper;
using ClothingStore.Domain.Entities;
using ClothingStore.Application.Features.User.Dtos;

namespace ClothingStore.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Users, UserResponseDto>();
        }
    }
}
