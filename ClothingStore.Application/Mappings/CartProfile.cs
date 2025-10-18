using AutoMapper;
using ClothingStore.Domain.Entities;
using ClothingStore.Application.Features.Carts.Dtos;

namespace ClothingStore.Application.Mappings
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            // Entity to DTO
            CreateMap<Cart, CartDto>();
            CreateMap<CartItem, CartItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Product.ImageUrl));

            // DTO to Entity (for commands)
            CreateMap<AddToCartDto, CartItem>()
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId));

            CreateMap<UpdateCartItemDto, CartItem>()
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId));
        }
    }
}
