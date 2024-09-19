using AutoMapper;

using ProjectUnitOfWork.APi.Dtos;
using ProjectUnitOfWork.Core.Models;



namespace ProjectUnitOfWork.API.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
      
        CreateMap<Employee, EmployeeDto>().ReverseMap();
        CreateMap<Employee, EmployeeCreateDto>().ReverseMap();
        CreateMap<Employee, EmployeeUpdateDto>().ReverseMap();



    }
}