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
        //private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(
            IProductRepository productRepository,
            IMapper mapper
            //IUnitOfWork unitOfWork
            )
        {
            _productRepository = productRepository;
            _mapper = mapper;
            //_unitOfWork = unitOfWork;
        }

        public async Task<Result<ProductDto>> Handle(ProductCreateCommand command, CancellationToken cancellationToken)
        {
            if (command.Product == null)
            {
                return Result.Failure<ProductDto>(new Error("InvalidRequest", "Product data is missing."));
            }
            var newProduct = Product.Create(command.Product.ProductName, command.Product.BriefDescription, command.Product.FullDescription, command.Product.TechnicalSpecifications, command.Product.ImageUrl, command.Product.Price, command.Product.CategoryId);
            await _productRepository.AddAsync(newProduct, cancellationToken);
            return Result.Success(_mapper.Map<ProductDto>(newProduct));
        }
    }
}