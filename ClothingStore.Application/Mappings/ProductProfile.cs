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
        }
    }
}
