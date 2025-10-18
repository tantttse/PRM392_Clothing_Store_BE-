using AutoMapper;
using ClothingStore.Application.Features.Orders.Dtos;
using ClothingStore.Domain.Entities;

namespace ClothingStore.Application.Features.Orders.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            // Entity -> DTO
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<OrderItem, OrderItemDto>();

            // DTO -> Entity (optional, allow manual creation)
            CreateMap<CreateOrderDto, Order>()
                .ConstructUsing(dto => Order.Create(
                    dto.UserId,
                    dto.CartId,
                    dto.PaymentMethod,
                    dto.BillingAddress,
                    new List<CartItem>() // empty, since items usually come from cart
                ));

            CreateMap<AddOrderItemDto, OrderItem>()
                .ConstructUsing(dto => new OrderItem(dto.ProductId, dto.Quantity, dto.UnitPrice));
        }
    }
}
