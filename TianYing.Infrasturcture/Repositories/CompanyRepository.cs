using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYing.Core.Entities;
using TianYing.Core.Helpers;
using TianYing.Core.Interfaces;
using TianYing.Core.ResourceParameters;
using TianYing.Infrasturcture.Databases;

namespace TianYing.Infrasturcture.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly MyContext _myContext;

        public CompanyRepository(MyContext myContext)
        {
            _myContext = myContext ?? throw new ArgumentNullException(nameof(myContext));
        }

        public void AddCompanyAsync(Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }

            company.Id = Guid.NewGuid();

            if (company.Employees != null)
            {
                foreach (var employee in company.Employees)
                {
                    employee.Id = Guid.NewGuid();
                }
            }

            _myContext.Companies.Add(company);

        }

        public async Task<bool> CompanyExistsAsync(Guid companyId)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }

            return await _myContext.Companies.AnyAsync(x => x.Id == companyId);
        }

        public void DeleteCompany(Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }

            _myContext.Companies.Remove(company);
        }

        public async Task<PagedList<Company>> GetCompaniesAsync(CompanyResourceParameter parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            //if (string.IsNullOrWhiteSpace(parameters.CompanyName) && string.IsNullOrWhiteSpace(parameters.SearchTerm))
            //{
            //    return await _myContext.Companies.ToListAsync();
            //}

            var queryExpression = _myContext.Companies as IQueryable<Company>;

            if (!string.IsNullOrWhiteSpace(parameters.CompanyName))
            {
                parameters.CompanyName = parameters.CompanyName.Trim();
                queryExpression = queryExpression.Where(x => x.Name == parameters.CompanyName);
            }

            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                parameters.SearchTerm = parameters.SearchTerm.Trim();
                queryExpression = queryExpression.Where(x => x.Name.Contains(parameters.SearchTerm) ||
                  x.Introduction.Contains(parameters.SearchTerm));
            }
  
            return await PagedList<Company>.Create(queryExpression,parameters.PageNumber,parameters.PageSize) ;
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync(IEnumerable<Guid> CompanyIds)
        {
            if (CompanyIds == null)
            {
                throw new ArgumentNullException(nameof(CompanyIds));
            }

            return await _myContext.Companies.Where(x => CompanyIds.Contains(x.Id)).OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<Company> GetCompanyAsync(Guid CompanyId)
        {
            if (CompanyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(CompanyId));
            }
            return await _myContext.Companies.FirstOrDefaultAsync(x => x.Id == CompanyId);
        }

        public void UpdateCompany(Company company)
        {
            // _myContext.Entry(company).State = EntityState.Modified;
        }

    }
}
