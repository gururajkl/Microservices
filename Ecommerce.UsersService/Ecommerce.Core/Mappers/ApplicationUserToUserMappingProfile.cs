using AutoMapper;
using Ecommerce.Core.DTO;
using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Mappers;

public class ApplicationUserToUserMappingProfile : Profile
{
    public ApplicationUserToUserMappingProfile()
    {
        CreateMap<ApplicationUser, UserDTO>()
            .ForMember(dest => dest.UserID, options => options.MapFrom(src => src.UserID))
            .ForMember(dest => dest.Email, options => options.MapFrom(src => src.Email))
            .ForMember(dest => dest.PersonName, options => options.MapFrom(src => src.PersonName))
            .ForMember(dest => dest.Gender, options => options.MapFrom(src => src.Gender));
    }
}
