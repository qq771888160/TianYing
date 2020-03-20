using System;
using System.Collections.Generic;
using System.Linq;
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

        public CompaniesController(ICompanyRepository companyRepository, IMapper mapper,IUnitOfWork unitOfWork)
        {
            _companyRepository = companyRepository ??
                throw new ArgumentNullException(nameof(companyRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        [HttpHead]
        public async Task<ActionResult<IEnumerable<CompanyResource>>> GetCompanies( [FromQuery]CompanyResourceParameter parameters)
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

            var companyResources = _mapper.Map<IEnumerable<CompanyResource>>(companies);

            //return NotFound();  // NotFound  空集合也认为是找到了 所以不是NotFound

            return Ok(companyResources); // 控制器级别的方法 200

            //return new JsonResult(companies);
        }

        [HttpGet("{companyId}", Name =nameof(GetCompany))]  // api/companies/{companyId}  控制器级别的URI

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
           await   unitOfWork.SaveAsync();

            var returnSource = _mapper.Map<CompanyResource>(entity);

            return CreatedAtRoute(nameof(GetCompany), new { companyId = returnSource.Id }, returnSource); // 创建Post 完成 返回201 Created 状态码
        }

        [HttpOptions]
        public IActionResult GetCompaniesOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, OPTIONS");
            return Ok();
        }
    }
}