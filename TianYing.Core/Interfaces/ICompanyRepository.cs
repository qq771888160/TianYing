using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TianYing.Core.Entities;

namespace TianYing.Core.Interfaces
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetCompaniesAsync();
        Task<Company> GetCompanyAsync(Guid CompanyId);

        Task<IEnumerable<Company>> GetCompaniesAsync(IEnumerable<Guid> CompanyIds);
        void AddCompanyAsync(Company company);
        void UpdateCompany(Company company);
        void DeleteCompany(Company company);
        Task<bool> CompanyExistsAsync(Guid companyId);         
    }
}
