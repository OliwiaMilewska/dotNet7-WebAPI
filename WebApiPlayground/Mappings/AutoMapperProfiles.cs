using AutoMapper;
using WebApiPlayground.Models.Domain;
using WebApiPlayground.Models.DTOs;

namespace WebApiPlayground.Mappings
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserDto, User>().ForMember(x => x.Name, opt => opt.MapFrom(z => z.FullName)).ReverseMap(); // Example

            CreateMap<RegionDto,Region>().ReverseMap(); // DTO -> Domain Model and Domain Model -> DTO because of ReverseMap()
            CreateMap<AddRegionRequestDto,Region>().ReverseMap();
            CreateMap<UpdateRegionDto,Region>().ReverseMap();
        }
    }

    // Example
    public class UserDto
    {
        public string FullName { get; set; }
    }

    public class User
    {
        public string Name { get; set; }
    }
}
