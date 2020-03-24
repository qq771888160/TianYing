using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TianYing.Core.Entities;
using TianYing.Core.Interfaces;
using TianYing.Core.ResourceParameters;
using TianYing.Infrasturcture.Resources;
using TianYing.Infrasturcture.Services;

namespace TianYing.Controllers
{
    [ApiController]
    [Route("api/companies/{companyId}/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IEmployeeRepository employeeRepository;
        private readonly ICompanyRepository companyRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IPropertyMappingService propertyMappingService;

        public EmployeesController(IMapper mapper,
                                                            IEmployeeRepository employeeRepository,
                                                            ICompanyRepository companyRepository,
                                                            IUnitOfWork unitOfWork,
                                                            IPropertyMappingService propertyMappingService
                                                         )
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            this.companyRepository = companyRepository;
            this.unitOfWork = unitOfWork;
            this.propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<EmployeeResource>>> GetEmployeesForCompany(Guid companyId, [FromQuery(Name = "gender")] string genderDisplay, string q)
        //{
        //    if (!await companyRepository.CompanyExistsAsync(companyId))
        //    {
        //        return NotFound();
        //    }

        //    var employees = await employeeRepository.GetEmployeesAsync(companyId, genderDisplay, q);
        //    var employeeResources = mapper.Map<IEnumerable<EmployeeResource>>(employees);

        //    return Ok(employeeResources);
        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeResource>>> GetEmployeesForCompany(Guid companyId, [FromQuery]  EmployeeResourcePatameter  patameters)
        {
            if (!await companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var employees = await employeeRepository.GetEmployeesAsync(companyId, patameters);
            var employeeResources = mapper.Map<IEnumerable<EmployeeResource>>(employees);

            return Ok(employeeResources);
        }


        [HttpGet("{employeeId}", Name = nameof(GetEmployeeForCompany))]
        public async Task<ActionResult<EmployeeResource>> GetEmployeeForCompany(Guid companyId, Guid employeeId)
        {
            if (!await companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var employee = await employeeRepository.GetEmployeeAsync(companyId, employeeId);
            if (employee == null)
            {
                return NotFound();
            }

            var employeeResource = mapper.Map<EmployeeResource>(employee);

            return Ok(employeeResource);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeResource>> CreateEmployeeForCompany(Guid companyId, EmployeeAddResource employee)
        {
            if (!await companyRepository.CompanyExistsAsync(companyId))
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

        [HttpPut("{employId}")]
        public async Task<ActionResult<EmployeeResource>> UpdateEmployeeForCompany(Guid companyId, Guid employeeId, EmployeeUpdateResource employee)
        {
            if (!await companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var employeeEntity = await employeeRepository.GetEmployeeAsync(companyId, employeeId);
            if (employeeEntity == null)
            {
                //return NotFound(); // put 新增
                var addResourceToEmployee = mapper.Map<Employee>(employee);
                addResourceToEmployee.Id = employeeId;

                employeeRepository.AddEmployee(companyId, addResourceToEmployee);

                await unitOfWork.SaveAsync();

                var retrunSource = mapper.Map<EmployeeResource>(addResourceToEmployee);

                return CreatedAtRoute(nameof(GetEmployeeForCompany), new
                {
                    companyId = companyId,
                    employeeId = retrunSource.Id
                }, retrunSource);

            }

            // 数据库entity 转化 updateresource
            // 把传进来的employee 更新 updateresource
            // updateresource 转化 数据库entity

            mapper.Map(employee, employeeEntity);

            employeeRepository.UpdateEmployee(employeeEntity);  // put 更新

            await unitOfWork.SaveAsync();

            return NoContent();  // 204 没有具体内容  200 OK 返回具体内容
        }

        [HttpPatch("{employeeId}")]
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid employeeId, JsonPatchDocument<EmployeeUpdateResource> patchDocument)  // 修改header application/json-patch+json
        {
            if (!await companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var employeeEntity = await employeeRepository.GetEmployeeAsync(companyId, employeeId);
            if (employeeEntity == null)
            {
                var employeeUpdateResource = new EmployeeUpdateResource();
                patchDocument.ApplyTo(employeeUpdateResource, ModelState);

                if (!TryValidateModel(employeeUpdateResource))  
                {
                    return ValidationProblem(ModelState);
                }

                var employeeAdd = mapper.Map<Employee>(employeeUpdateResource);
                employeeAdd.Id = employeeId;

                employeeRepository.AddEmployee(employeeId, employeeAdd);
                 await unitOfWork.SaveAsync();

                var resourceReturn = mapper.Map<EmployeeResource>(employeeAdd);

                return CreatedAtRoute(nameof(GetEmployeeForCompany), new
                {
                    companyId = companyId,
                    employeeId = resourceReturn.Id
                }, resourceReturn);

            }

            var entityToPatch = mapper.Map<EmployeeUpdateResource>(employeeEntity);

            //需要处理验证错误
            patchDocument.ApplyTo(entityToPatch,ModelState);

            if (!TryValidateModel(entityToPatch))  // 将patchDocument类型 验证
            {
                return ValidationProblem(ModelState);
            }

            mapper.Map(entityToPatch, employeeEntity);

            employeeRepository.UpdateEmployee(employeeEntity);

            await unitOfWork.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{employeeId}")]
        public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId,Guid employeeId)
        {
            if (!await companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var employee = await employeeRepository.GetEmployeeAsync(companyId, employeeId);
            if (employee == null)
            {
                return NotFound();
            }

            employeeRepository.DeleteEmployee(employee);

           await  unitOfWork.SaveAsync();

            return NoContent();

        }

        public override ActionResult ValidationProblem([ActionResultObjectValue]ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();

            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }
}
 