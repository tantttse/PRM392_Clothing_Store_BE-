using AutoMapper;
using ClothingStore.Application.Features.Products.Dtos;
using ClothingStore.Domain.Entities;
using ClothingStore.Domain.Repositories;
using FluentValidation;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Common.ResponseModel;

namespace ClothingStore.Application.Features.Products.Commands
{
    public class ProductCreateDtoValidator : AbstractValidator<ProductCreateDto>
    {
        public ProductCreateDtoValidator()
        {
            RuleFor(x => x.ProductName).NotEmpty();
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.CategoryId).NotEmpty();
        }
    }

    public class CreateProductCommandHandler : ICommandHandler<ProductCreateCommand, ProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public CreateProductCommandHandler(
            IProductRepository productRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Result<ProductDto>> Handle(ProductCreateCommand command, CancellationToken cancellationToken)
        {
            if (command.Product == null)
            {
                return Result.Failure<ProductDto>(
                    new Error("InvalidRequest", "Product data is missing."));
            }

            // Use AutoMapper to map DTO -> Entity (via ProductProfile)
            var newProduct = _mapper.Map<Product>(command.Product);

            await _productRepository.AddAsync(newProduct, cancellationToken);

            // Map Entity -> DTO for response
            return Result.Success(_mapper.Map<ProductDto>(newProduct));
        }
    }
}
