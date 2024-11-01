using AutoMapper;
using Demo.DAL.Entities;
using PL.Models;

namespace PL.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<EmployeeVM, Employee>().ReverseMap();
            CreateMap<DepartmentVM, Department>().ReverseMap();
            //CreateMap<Employee, EmployeeVM>();
        }
    }
}
