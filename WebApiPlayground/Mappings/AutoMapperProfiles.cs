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

            CreateMap<WalkDto, Walk>().ReverseMap();
            CreateMap<ModifyWalkDto,Walk>().ReverseMap();

            CreateMap<DifficultyDto,Difficulty>().ReverseMap();

            CreateMap<ImageUploadRequestDto, Image>()
            .ForMember(dest => dest.File, opt => opt.MapFrom(src => src.File))
            .ForMember(dest => dest.FileExtension, opt => opt.MapFrom(src => Path.GetExtension(src.File.FileName)))
            .ForMember(dest => dest.FileSizeInBytes, opt => opt.MapFrom(src => src.File.Length))
            .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
            .ForMember(dest => dest.FileDescription, opt => opt.MapFrom(src => src.FileDescription))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.FilePath, opt => opt.Ignore());
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
