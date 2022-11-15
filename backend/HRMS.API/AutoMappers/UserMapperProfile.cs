using AutoMapper;
using HRMS.API.Models;
using HRMS.Entities;
using System.Linq;
namespace HRMS.API.AutoMappers
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<Auditable, Auditable>()
                .ForMember(dest => dest.UserCreated, act => act.Ignore())
                .ForMember(dest => dest.UserModified, act => act.Ignore())
                .ForMember(dest => dest.DateCreated, act => act.Ignore())
                .ForMember(dest => dest.DateModified, act => act.Ignore());

            CreateMap<ApplicationUser, UserDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName == null ? src.UserName : src.FullName))
                .ForMember(dest => dest.UserStatus, opt => opt.MapFrom(src => src.UserStatus.ToString()))
                .ForMember(dest => dest.ProfileStatus, opt => opt.MapFrom(src => src.ProfileStatus.ToString()))
                .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.UserRoles.Select(s => s.Role.Name)));
              //  .IncludeBase<IAuditable, AuditableDto>();



            CreateMap<UserProfile, UserProfileDto>();

            CreateMap<UserProfileDto, UserProfile>()
                .ReverseMap();
            CreateMap<UserProfile, UserProfile>()
              .ReverseMap();

            //UserBasicInfo
            CreateMap<UserBasicInfo, UserBasicInfoDto>()
                .ForMember(dest => dest.GuardianType, opt => opt.MapFrom(src => src.GuardianType.ToString()))
                .ForMember(dest => dest.ProfilePictureUrl, opt => opt.MapFrom(src => src.ProfileImageUrl))

                .ReverseMap();

            CreateMap<UserBasicInfo, UserBasicInfo>()
                .IncludeBase<Auditable, Auditable>()
                .ReverseMap();

            //UserContactInfo
            CreateMap<UserContactInfo, UserContactInfoDto>()
               .ReverseMap();

            CreateMap<UserContactInfo, UserContactInfo>()
                .IncludeBase<Auditable, Auditable>()
                .ReverseMap();

        }
    }
}
