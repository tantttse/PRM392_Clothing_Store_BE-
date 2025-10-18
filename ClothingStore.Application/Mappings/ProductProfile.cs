using AutoMapper;
using ClothingStore.Domain.Entities;
using ClothingStore.Application.Features.User.Dtos;
using ClothingStore.Application.Features.Products.Dtos;

namespace ClothingStore.Application.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductCreateDto, Product>();
            CreateMap<ProductUpdateDto, Product>();
             // Entity -> DTO
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName));

            // DTO -> Entity (Create)
            CreateMap<ProductCreateDto, Product>()
                .ConstructUsing(dto => Product.Create(
                    dto.ProductName,
                    dto.BriefDescription,
                    dto.FullDescription,
                    dto.TechnicalSpecifications,
                    dto.ImageUrl,
                    dto.Price,
                    dto.CategoryId
                ));

            // DTO -> Entity (Update)
            CreateMap<ProductUpdateDto, Product>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
