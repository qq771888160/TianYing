using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

        public CompanyCollectionsController(IMapper mapper, ICompanyRepository companyRepository)
        {
            this.mapper = mapper;
            this.companyRepository = companyRepository;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<CompanyResource>>> CreateCompanyCollection()
        {

        }
    }
}