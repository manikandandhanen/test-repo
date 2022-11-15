using AutoMapper;
using HRMS.API.Models;
using HRMS.Entities;

namespace HRMS.API.AutoMappers
{
    public class RoleMapperProfile : Profile
    {
        public RoleMapperProfile()
        {
            CreateMap<ApplicationRole, RoleDto>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();
                //.ReverseMap();

            //CreateMap<Employee, EditEmployeeModel>()
            //   .ForMember(dest => dest.ConfirmEmail,
            //              opt => opt.MapFrom(src => src.Email));
            //CreateMap<EditEmployeeModel, Employee>();
        }
    }
}
