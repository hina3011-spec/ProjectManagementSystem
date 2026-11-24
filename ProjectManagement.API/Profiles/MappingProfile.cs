using AutoMapper;
using ProjectManagement.API.Models;
using ProjectManagement.API.DTOs;

namespace ProjectManagement.API.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeResponseDto>()
                .ForMember(
                    dest => dest.DepartmentName,
                    opt => opt.MapFrom(
                        src => src.Department.DepartmentName
                    )
                );

            //create
            CreateMap<EmployeeCreateDto, Employee > ()
            .ForMember(dest => dest.EmployeeId, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.RoleId, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

            //update
            CreateMap<EmployeeUpdateDto, Employee>()
            .ForMember(dest => dest.EmployeeId, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.RoleId, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));
        }

    }
}