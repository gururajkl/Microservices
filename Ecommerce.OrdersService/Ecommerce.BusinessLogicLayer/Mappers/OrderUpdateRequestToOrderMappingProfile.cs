using Ecommerce.BusinessLogicLayer.DTO;
using Ecommerce.DataAccessLayer.Entities;

namespace Ecommerce.BusinessLogicLayer.Mappers;

public class OrderUpdateRequestToOrderMappingProfile : Profile
{
    public OrderUpdateRequestToOrderMappingProfile()
    {
        CreateMap<OrderUpdateRequest, Order>()
            .ForMember(dest => dest.UserID, options => options.MapFrom(src => src.UserID))
            .ForMember(dest => dest.OrderID, options => options.MapFrom(src => src.OrderID))
            .ForMember(dest => dest.OrderDate, options => options.MapFrom(src => src.OrderDate))
            .ForMember(dest => dest.OrderItems, options => options.MapFrom(src => src.OrderItems))
            .ForMember(dest => dest._id, options => options.Ignore())
            .ForMember(dest => dest.TotalBill, options => options.Ignore());
    }
}
