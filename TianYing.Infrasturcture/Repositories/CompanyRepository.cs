using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYing.Core.Entities;
using TianYing.Core.Interfaces;
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

            foreach (var employee in company.Employees)
            {
                employee.Id = Guid.NewGuid();
            }

            _myContext.Companies.Add(company);

        }

        public async Task<bool> CompanyExistsAsync(Guid companyId)
        {
            if(companyId==Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }

            return await _myContext.Companies.AnyAsync(x => x.Id == companyId);
        }

        public void DeleteCompany(Company company)
        {
           if(company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }

            _myContext.Companies.Remove(company);
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync()
        {
            return await _myContext.Companies.ToListAsync();
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
            return await _myContext.Companies.FirstOrDefaultAsync(x=>x.Id ==CompanyId);
        }

        public void UpdateCompany(Company company)
        {
           // _myContext.Entry(company).State = EntityState.Modified;
        }

    }
}
