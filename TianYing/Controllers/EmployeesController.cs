using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TianYing.Core.Entities;
using TianYing.Core.Interfaces;
using TianYing.Infrasturcture.Resources;

namespace TianYing.Controllers
{
    [ApiController]
    [Route("api/companies/{companyId}/employees")]
    public class EmployeesController:ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IEmployeeRepository employeeRepository;
        private readonly ICompanyRepository companyRepository;
        private readonly IUnitOfWork unitOfWork;

        public EmployeesController(IMapper mapper,
                                                            IEmployeeRepository employeeRepository,
                                                            ICompanyRepository companyRepository,
                                                            IUnitOfWork unitOfWork
                                                         )
        {
            this.mapper = mapper ?? throw  new ArgumentNullException(nameof(mapper));
            this.employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            this.companyRepository = companyRepository;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeResource>>> GetEmployeesForCompany(Guid companyId,[FromQuery(Name ="gender")] string genderDisplay,string q)
        {
             if( ! await companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var employees = await  employeeRepository.GetEmployeesAsync(companyId,genderDisplay,q);
            var employeeResources = mapper.Map<IEnumerable<EmployeeResource>>(employees);

            return Ok(employeeResources);
        }

        [HttpGet("{employeeId}",Name =nameof(GetEmployeeForCompany))]
        public async Task<ActionResult<EmployeeResource>> GetEmployeeForCompany(Guid companyId,Guid employeeId)
        {
            if (!await companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var employee = await employeeRepository.GetEmployeeAsync(companyId, employeeId);
            if(employee == null)
            {
                return NotFound();
            }

            var employeeResource = mapper.Map<EmployeeResource>(employee);

            return Ok(employeeResource);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeResource>> CreateEmployeeForCompany(Guid companyId,EmployeeAddResource employee)
        {
            if(! await companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var entity = mapper.Map<Employee>(employee);

            employeeRepository.AddEmployee(companyId, entity);
            await unitOfWork.SaveAsync();

            var retrunSource = mapper.Map<EmployeeResource>(entity);

            return CreatedAtRoute(nameof(GetEmployeeForCompany), new
            {
                companyId = companyId,
                employeeId = retrunSource.Id
            }, retrunSource);
        }

    }
}
