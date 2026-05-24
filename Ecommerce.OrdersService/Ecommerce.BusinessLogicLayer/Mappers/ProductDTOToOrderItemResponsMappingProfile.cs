using Ecommerce.BusinessLogicLayer.DTO;

namespace Ecommerce.BusinessLogicLayer.Mappers;

public class ProductDTOToOrderItemResponsMappingProfile : Profile
{
    public ProductDTOToOrderItemResponsMappingProfile()
    {
        CreateMap<ProductDTO, OrderItemResponse>()
            .ForMember(dest => dest.ProductName, option => option.MapFrom(product => product.ProductName))
            .ForMember(dest => dest.Category, option => option.MapFrom(product => product.Category));
    }
}
