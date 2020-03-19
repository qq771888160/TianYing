using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TianYing.Core.Entities;

namespace TianYing.Core.Interfaces
{
  public  interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId,string genderDisplay,string q);
        Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId);
        void AddEmployee(Guid companyId, Employee employee);
        void UpdateEmployee(Employee employee);
        void DeleteEmployee(Employee employee);
    }
}
