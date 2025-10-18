using AutoMapper;
using ClothingStore.Application.Features.Products.Dtos;
using ClothingStore.Domain.Repositories;
using FluentValidation;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Common.ResponseModel;

namespace ClothingStore.Application.Features.Products.Commands
{
    public class ProductUpdateDtoValidator : AbstractValidator<ProductUpdateDto>
    {
        public ProductUpdateDtoValidator()
        {
            RuleFor(x => x.ProductName).NotEmpty();
            RuleFor(x => x.Price).GreaterThan(0);
            // CategoryId is optional in updates, so you may want to remove NotEmpty() here
        }
    }

    public class UpdateProductCommandHandler : ICommandHandler<ProductUpdateCommand, ProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public UpdateProductCommandHandler(
            IProductRepository productRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Result<ProductDto>> Handle(ProductUpdateCommand command, CancellationToken cancellationToken)
        {
            var existingProduct = await _productRepository.GetByIdAsync(command.Id, cancellationToken);
            if (existingProduct == null)
            {
                return Result.Failure<ProductDto>(
                    new Error("ProductNotFound", "Product not found."));
            }

            // Use AutoMapper to apply updates from DTO -> Entity
            _mapper.Map(command.Product, existingProduct);

            _productRepository.Update(existingProduct, cancellationToken);

            return Result.Success(_mapper.Map<ProductDto>(existingProduct));
        }
    }
}
