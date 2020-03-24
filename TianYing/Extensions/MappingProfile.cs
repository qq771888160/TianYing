using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TianYing.Core.Entities;
using TianYing.Infrasturcture.Resources;

namespace TianYing.Extensions
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyResource>()
                .ForMember(
                dest => dest.CompanyName, opt => opt.MapFrom(src => src.Name)
                );
            CreateMap<CompanyAddResource, Company>();

            CreateMap<Employee, EmployeeResource>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest=>dest.GenderDisplay,opt=>opt.MapFrom(src=>src.Gender.ToString()))
                .ForMember(dest=>dest.Age,opt=>opt.MapFrom(src=>DateTime.Now.Year-src.DateofBirth.Year));
            CreateMap<EmployeeAddResource, Employee>();
            CreateMap<EmployeeUpdateResource, Employee>();
            CreateMap< Employee, EmployeeUpdateResource>();

        }
    }
}
