using Ecommerce.BusinessLogicLayer.DTO;
using Ecommerce.DataAccessLayer.Entities;

namespace Ecommerce.BusinessLogicLayer.Mappers;

public class OrderToOrderResponseMappingProfile : Profile
{
    public OrderToOrderResponseMappingProfile()
    {
        CreateMap<Order, OrderResponse>()
            .ForMember(dest => dest.OrderID, options => options.MapFrom(src => src.OrderID))
            .ForMember(dest => dest.UserID, options => options.MapFrom(src => src.UserID))
            .ForMember(dest => dest.OrderDate, options => options.MapFrom(src => src.OrderDate))
            .ForMember(dest => dest.TotalBill, options => options.MapFrom(src => src.TotalBill))
            .ForMember(dest => dest.OrderItems, options => options.MapFrom(src => src.OrderItems));
    }
}
