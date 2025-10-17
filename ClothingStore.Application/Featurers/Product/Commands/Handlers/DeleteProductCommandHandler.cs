using AutoMapper;
using ClothingStore.Application.Features.Products.Dtos;
using ClothingStore.Application.Features.User.Dtos;
using ClothingStore.Domain.Entities;
using ClothingStore.Domain.Repositories;
using MediatR;
using Shared.Application.Abstractions.Authentication;
using Shared.Application.Abstractions.DTOs;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Common.ResponseModel;

namespace ClothingStore.Application.Features.Products.Commands
{

    
    public class DeleteProductCommandHandler : ICommandHandler<ProductDeleteCommand>
    {
        private readonly IProductRepository _productRepository;
        //private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(
            IProductRepository productRepository
            //IUnitOfWork unitOfWork
            )
        {
            _productRepository = productRepository;
            //_unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(ProductDeleteCommand command, CancellationToken cancellationToken)
        {
            var existingProduct = await _productRepository.GetByIdAsync(command.Id, cancellationToken);
            if (existingProduct == null)
            {
                return Result.Failure<ProductDeleteResponseDto>(new Error("ProductNotFound", "Product not found."));
            }
            _productRepository.Delete(existingProduct, cancellationToken);
            return Result.Success();
        }
    }
}