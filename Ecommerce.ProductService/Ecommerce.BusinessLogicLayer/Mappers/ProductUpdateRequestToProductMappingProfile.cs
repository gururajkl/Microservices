using AutoMapper;
using Ecommerce.BusinessLogicLayer.DTO;
using Ecommerce.DataAccessLayer.Entities;

namespace Ecommerce.BusinessLogicLayer.Mappers;

public class ProductUpdateRequestToProductMappingProfile : Profile
{
    public ProductUpdateRequestToProductMappingProfile()
    {
        CreateMap<ProductUpdateRequest, Product>()
            .ForMember(dest => dest.ProductName, options => options.MapFrom(src => src.ProductName))
            .ForMember(dest => dest.Category, options => options.MapFrom(src => src.Category))
            .ForMember(dest => dest.UnitPrice, options => options.MapFrom(src => src.UnitPrice))
            .ForMember(dest => dest.QuantityInStock, options => options.MapFrom(src => src.QuantityInStock))
            .ForMember(dest => dest.ProductID, options => options.MapFrom(src => src.ProductID));
    }
}
