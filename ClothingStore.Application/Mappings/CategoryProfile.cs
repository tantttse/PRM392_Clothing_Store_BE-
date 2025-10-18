using AutoMapper;
using ClothingStore.Application.Features.Categories.Dtos;
using ClothingStore.Application.Features.Products.Dtos;
using ClothingStore.Domain.Entities;

namespace ClothingStore.Application.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));

            CreateMap<Product, ProductLightWeightDto>();
        }
    }
}
