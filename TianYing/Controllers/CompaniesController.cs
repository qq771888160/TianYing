using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TianYing.Core.Entities;
using TianYing.Core.Interfaces;
using TianYing.Core.ResourceParameters;
using TianYing.Infrasturcture.Resources;

namespace TianYing.Controllers
{
    [ApiController]
    [Route("api/companies")]
    // [Route("api/[controller]")] 相当于 api/companies
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly IEmployeeRepository employeeRepository;

        public CompaniesController(ICompanyRepository companyRepository, IMapper mapper, IUnitOfWork unitOfWork, IEmployeeRepository employeeRepository)
        {
            _companyRepository = companyRepository ??
                throw new ArgumentNullException(nameof(companyRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            this.unitOfWork = unitOfWork;
            this.employeeRepository = employeeRepository;
        }

        [HttpGet(Name = nameof(GetCompanies))]
        [HttpHead]
        public async Task<ActionResult<IEnumerable<CompanyResource>>> GetCompanies([FromQuery]CompanyResourceParameter parameters)
        {
            var companies = await _companyRepository.GetCompaniesAsync(parameters);

            //var companyResource = new List<CompanyResource>();

            //foreach (var company in companies)
            //{
            //    companyResource.Add(new CompanyResource // 属性多的时候赋值很麻烦 使用automap
            //    {
            //        Id = company.Id,
            //        CompanyName = company.Name
            //    });

            //}

            var PerviousPageLink = companies.HasPrevious ? CreateCompaniesResourceUri(parameters, ResourceUriType.PreviousPage) : null;

            var nextPageLink = companies.HasNext ? CreateCompaniesResourceUri(parameters, ResourceUriType.NextPage) : null;

            var paginationMetadata = new 
            {
                    totalCount =companies.TotalCount,
                    pageSize =companies.PageSize,
                    currentPage =companies.CurrentPage,
                    totalPages=companies.TotalPages,
                    PerviousPageLink,
                    nextPageLink
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata,new JsonSerializerOptions
            { 
                    Encoder=JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }));

            var companyResources = _mapper.Map<IEnumerable<CompanyResource>>(companies);

            //return NotFound();  // NotFound  空集合也认为是找到了 所以不是NotFound

            return Ok(companyResources); // 控制器级别的方法 200

            //return new JsonResult(companies);
        }

        [HttpGet("{companyId}", Name = nameof(GetCompany))]  // api/companies/{companyId}  控制器级别的URI

        //[HttpGet]
        //[Route("{companId}")]   这种写法也行
        public async Task<ActionResult<CompanyResource>> GetCompany(Guid companyId)
        {
            // 这种写法当同时又操作删除该实体 后面就查不到了
            //var exist = await _companyRepository.CompanyExistsAsync(companyId);
            //if(!exist)
            //{
            //    return NotFound();
            //}

            var company = await _companyRepository.GetCompanyAsync(companyId);
            if (company == null)
            {
                return NotFound();
            }

            var companyResource = _mapper.Map<CompanyResource>(company);
            return Ok(companyResource); // 控制器级别的方法 200
        }

        [HttpPost]
        public async Task<ActionResult<CompanyResource>> CreateCompany(CompanyAddResource company)
        {
            //if(company == null)
            //{
            //    return BadRequest();  // 客户端错误  400badrequest
            //}  

            var entity = _mapper.Map<Company>(company);
            _companyRepository.AddCompanyAsync(entity);
            await unitOfWork.SaveAsync();

            var returnSource = _mapper.Map<CompanyResource>(entity);

            return CreatedAtRoute(nameof(GetCompany), new { companyId = returnSource.Id }, returnSource); // 创建Post 完成 返回201 Created 状态码
        }

        [HttpOptions]
        public IActionResult GetCompaniesOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, OPTIONS");
            return Ok();
        }

        [HttpDelete("{companyId}")]
        public async Task<IActionResult> DeleteCompany(Guid companyId)
        {
            var companyEntity = await _companyRepository.GetCompanyAsync(companyId);

            if (companyEntity == null)
            {
                return NotFound();
            }

            await employeeRepository.GetEmployeesAsync(companyId, null);

            _companyRepository.DeleteCompany(companyEntity);
            await unitOfWork.SaveAsync();

            return NoContent();
        }

        public string CreateCompaniesResourceUri(CompanyResourceParameter parameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetCompanies), new
                    {
                        pageNumber = parameters.PageNumber - 1,
                        pageSize = parameters.PageSize,
                        companyName = parameters.CompanyName,
                        searchTerm = parameters.SearchTerm
                    });
                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetCompanies), new
                    {
                        pageNumber = parameters.PageNumber + 1,
                        pageSize = parameters.PageSize,
                        companyName = parameters.CompanyName,
                        searchTerm = parameters.SearchTerm
                    });
                default:
                    return Url.Link(nameof(GetCompanies), new
                    {
                        pageNumber = parameters.PageNumber,
                        pageSize = parameters.PageSize,
                        companyName = parameters.CompanyName,
                        searchTerm = parameters.SearchTerm
                    });
            }
        }
    }
}