using System.Linq.Expressions;
using AutoMapper;
using ClothingStore.Application.Features.Products.Dtos;
using ClothingStore.Application.Features.Products.Queries;
using ClothingStore.Application.Features.User.Dtos;
using ClothingStore.Domain.Entities;
using ClothingStore.Domain.Repositories;
using MediatR;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Common.ResponseModel;
using Shared.Helpers;

namespace ClothingStore.Application.Features.User.Queries
{
    public class GetProductListQueryHandler
    : IQueryHandler<GetProductListQuery, IEnumerable<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductListQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<ProductDto>>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
        {
            var pageIndex = request.Filter.PageIndex;
            var pageSize = request.Filter.PageSize;
            var orderByColumn = ExpressionBuilder.BuildOrderByExpression<Product>(request.Filter.OrderBy ?? "created_at");
            var isAscending = request.Filter.IsAscending;
            var categoryId = request.Filter.CategoryId;
            var minPrice = request.Filter.MinPrice;
            var maxPrice = request.Filter.MaxPrice;
            var searchTerm = request.Filter.SearchTerm;
            Expression<Func<Product, bool>> predicate = e => true;

            if (categoryId.HasValue)
            {
                predicate = predicate.And(e => e.CategoryId == categoryId.Value);
            }
            if (minPrice.HasValue)
            {
                predicate = predicate.And(e => e.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                predicate = predicate.And(e => e.Price <= maxPrice.Value);
            }
            if (!string.IsNullOrEmpty(searchTerm))
            {
                predicate = predicate.And(e => e.ProductName.Contains(searchTerm) ||
                                               (e.BriefDescription != null && e.BriefDescription.Contains(searchTerm)) ||
                                               (e.FullDescription != null && e.FullDescription.Contains(searchTerm)) ||
                                               (e.TechnicalSpecifications != null && e.TechnicalSpecifications.Contains(searchTerm)) ||
                                               (e.Category != null && e.Category.CategoryName.Contains(searchTerm))
                                               );
            }
            var (items, totalCount) = await _productRepository.GetPagedAsync(
            pageIndex,
            pageSize,
            predicate,
            orderByColumn,
            isAscending,
            cancellationToken
            );

            return Result.Success(_mapper.Map<IEnumerable<ProductDto>>(items));
        }
    }
}
