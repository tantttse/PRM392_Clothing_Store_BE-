using AutoMapper;
using ClothingStore.Application.Features.Products.Dtos;
using ClothingStore.Application.Features.User.Dtos;
using ClothingStore.Domain.Entities;
using ClothingStore.Domain.Repositories;
using FluentValidation;
using MediatR;
using Shared.Application.Abstractions.Authentication;
using Shared.Application.Abstractions.DTOs;
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
        RuleFor(x => x.CategoryId).NotEmpty();
    }
}
    public class UpdateProductCommandHandler : ICommandHandler<ProductUpdateCommand, ProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        //private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(
            IProductRepository productRepository,
            IMapper mapper
            //IUnitOfWork unitOfWork
            )
        {
            _productRepository = productRepository;
            _mapper = mapper;
            //_unitOfWork = unitOfWork;
        }

        public async Task<Result<ProductDto>> Handle(ProductUpdateCommand command, CancellationToken cancellationToken)
        {
            var existingProduct = await _productRepository.GetByIdAsync(command.Id, cancellationToken);
            if (existingProduct == null)
            {
                return Result.Failure<ProductDto>(new Error("ProductNotFound", "Product not found."));
            }

            existingProduct.UpdateDetails(command.Product.ProductName, command.Product.BriefDescription, command.Product.FullDescription, command.Product.TechnicalSpecifications, command.Product.ImageUrl, command.Product.Price , command.Product.CategoryId);
            _productRepository.Update(existingProduct, cancellationToken);
            return Result.Success(_mapper.Map<ProductDto>(existingProduct));
        }
    }
}