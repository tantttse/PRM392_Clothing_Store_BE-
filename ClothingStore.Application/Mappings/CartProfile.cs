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
            CreateMap<Cart, CartDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items.OrderBy(i => i.CreatedAt)));

            //if use this as mapper then have to include the product to properly map in repo
            // CreateMap<CartItem, CartItemDto>() 
            //     .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
            //     .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Product.ImageUrl));

            // Map directly from CartItem fields
            CreateMap<CartItem, CartItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));

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
