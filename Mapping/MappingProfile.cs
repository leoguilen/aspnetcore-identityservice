using AuthenticationClientService.API.Models;
using AutoMapper;

namespace AuthenticationClientService.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserModel>().ReverseMap();
        }
    }
}
