using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TianYing.Core.Entities;
using TianYing.Core.Helpers;
using TianYing.Core.Interfaces;
using TianYing.Infrasturcture.Resources;

namespace TianYing.Controllers
{
    [ApiController]
    [Route("api/companycollections")]
    public class CompanyCollectionsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ICompanyRepository companyRepository;
        private readonly IUnitOfWork unitOfWork;

        public CompanyCollectionsController(IMapper mapper, ICompanyRepository companyRepository, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.companyRepository = companyRepository;
            this.unitOfWork = unitOfWork;
        }

        //1,2,3,4  数组
        //key1=value1,key2=value2,key3=value3,key4=value4
        [HttpGet("({ids})",Name =nameof(GetCompanyCollection))]
        public async Task<IActionResult> GetCompanyCollection([FromRoute] [ModelBinder(BinderType =typeof(ArrayModelBinder))]IEnumerable<Guid> ids)
        {
            if (ids ==null)
            {
                return BadRequest();
            }

            var entities = await companyRepository.GetCompaniesAsync(ids);

            if (ids.Count()!=entities.Count())
            {
                return NotFound();
            }

            var companyResources = mapper.Map<IEnumerable<CompanyResource>>(entities);

            return Ok(companyResources);

        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<CompanyResource>>> CreateCompanyCollection([FromBody]IEnumerable<CompanyAddResource> companyAddResource)
        {
            var companies = mapper.Map<IEnumerable<Company>>(companyAddResource);

            foreach (var company in companies)
            {
                companyRepository.AddCompanyAsync(company);
                
            }
            await unitOfWork.SaveAsync();

            var companyResources = mapper.Map<IEnumerable<CompanyResource>>(companies);

            var idsString = string.Join(",", companyResources.Select(x => x.Id));

            return CreatedAtRoute(nameof(GetCompanyCollection),new { ids=idsString}, companyResources);
        }
    }
}