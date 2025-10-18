using System.Linq.Expressions;
using AutoMapper;
using ClothingStore.Application.Features.Products.Dtos;
using ClothingStore.Application.Features.Products.Queries;
using ClothingStore.Domain.Entities;
using ClothingStore.Domain.Repositories;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Common.ResponseModel;
using Shared.Helpers;

namespace ClothingStore.Application.Features.Products.Queries
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
            var filter = request.Filter;
            var pageIndex = filter.PageIndex;
            var pageSize = filter.PageSize;
            var orderByColumn = ExpressionBuilder.BuildOrderByExpression<Product>(filter.OrderBy ?? "created_at");
            var isAscending = filter.IsAscending;

            Expression<Func<Product, bool>> predicate = e => true;

            if (filter.CategoryId.HasValue)
                predicate = predicate.And(e => e.CategoryId == filter.CategoryId.Value);

            if (filter.MinPrice.HasValue)
                predicate = predicate.And(e => e.Price >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                predicate = predicate.And(e => e.Price <= filter.MaxPrice.Value);

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                var term = filter.SearchTerm;
                predicate = predicate.And(e =>
                    e.ProductName.Contains(term) ||
                    (e.BriefDescription != null && e.BriefDescription.Contains(term)) ||
                    (e.FullDescription != null && e.FullDescription.Contains(term)) ||
                    (e.TechnicalSpecifications != null && e.TechnicalSpecifications.Contains(term)) ||
                    (e.Category != null && e.Category.CategoryName.Contains(term))
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
