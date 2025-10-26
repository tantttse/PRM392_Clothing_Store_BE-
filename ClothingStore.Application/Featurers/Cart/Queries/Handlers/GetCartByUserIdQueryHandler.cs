using AutoMapper;
using ClothingStore.Application.Features.Carts.Dtos;
using ClothingStore.Domain.Repositories;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Common.ResponseModel;

namespace ClothingStore.Application.Features.Carts.Queries;

public class GetCartByUserIdQueryHandler : IQueryHandler<GetCartByUserIdQuery, CartDto>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    public GetCartByUserIdQueryHandler(ICartRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    public async Task<Result<CartDto>> Handle(GetCartByUserIdQuery query, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetActiveCartByUserIdAsync(query.UserId, cancellationToken);
        if (cart == null)
            return Result.Failure<CartDto>(new Error("CartNotFound", "No active cart found for this user."));

        return Result.Success(_mapper.Map<CartDto>(cart));
    }
}
