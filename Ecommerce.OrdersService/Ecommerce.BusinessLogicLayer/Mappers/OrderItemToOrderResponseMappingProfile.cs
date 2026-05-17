using Ecommerce.BusinessLogicLayer.DTO;
using Ecommerce.DataAccessLayer.Entities;

namespace Ecommerce.BusinessLogicLayer.Mappers;

public class OrderItemToOrderResponseMappingProfile : Profile
{
    public OrderItemToOrderResponseMappingProfile()
    {
        CreateMap<OrderItem, OrderItemResponse>()
            .ForMember(dest => dest.ProductID, options => options.MapFrom(src => src.ProductID))
            .ForMember(dest => dest.UnitPrice, options => options.MapFrom(src => src.UnitPrice))
            .ForMember(dest => dest.Quantity, options => options.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.TotalPrice, options => options.MapFrom(src => src.TotalPrice));
    }
}
