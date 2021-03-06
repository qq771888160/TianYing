﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYing.Core.Entities;
using TianYing.Core.Interfaces;
using TianYing.Core.ResourceParameters;
using TianYing.Infrasturcture.Databases;
using TianYing.Infrasturcture.Resources;
using TianYing.Infrasturcture.Services;

namespace TianYing.Infrasturcture.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly MyContext _myContext;
        private readonly IPropertyMappingService propertyMappingService;

        public EmployeeRepository(MyContext myContext , IPropertyMappingService propertyMappingService)
        {
            _myContext = myContext;
            this.propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
        }
        public void AddEmployee(Guid companyId, Employee employee)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }

            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            employee.CompanyId = companyId;
            _myContext.Employees.Add(employee);
        }

        public void DeleteEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            _myContext.Employees.Remove(employee);
        }

        public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }

            if (employeeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(employeeId));
            }

            return await _myContext.Employees
                .Where(x => x.CompanyId == companyId && x.Id == employeeId)
                .FirstOrDefaultAsync();
        }

        //public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId,string genderDisplay,string q)
        //{
        //    if (companyId == Guid.Empty)
        //    {
        //        throw new ArgumentNullException(nameof(companyId));
        //    }

        //    if(string.IsNullOrWhiteSpace(genderDisplay) && string.IsNullOrWhiteSpace(q))
        //    {
        //        return await _myContext.Employees
        //        .Where(x => x.CompanyId == companyId)
        //        .OrderBy(x => x.EmployeeNo)
        //        .ToListAsync();
        //    }

        //    var items = _myContext.Employees.Where(x => x.CompanyId == companyId);

        //    if (!string.IsNullOrWhiteSpace(genderDisplay))
        //    {
        //        genderDisplay = genderDisplay.Trim();
        //        var gender = Enum.Parse<Gender>(genderDisplay);

        //        items = items.Where(x=>x.Gender == gender);
        //    }

        //    if (!string.IsNullOrWhiteSpace(q))
        //    {
        //        q = q.Trim();

        //        items = items.Where(x => x.EmployeeNo.Contains(q) ||
        //          x.FirstName.Contains(q) ||
        //          x.LastName.Contains(q));
        //    }

        //    return await items.OrderBy(x => x.EmployeeNo)
        //        .ToListAsync();
        //}

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, EmployeeResourcePatameter patameters)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }

            //if (string.IsNullOrWhiteSpace(patameters.Gender) && string.IsNullOrWhiteSpace(patameters.Q))
            //{
            //    return await _myContext.Employees
            //    .Where(x => x.CompanyId == companyId)
            //    .OrderBy(x => x.EmployeeNo)
            //    .ToListAsync();
            //}

            var items = _myContext.Employees.Where(x => x.CompanyId == companyId);

            if (!string.IsNullOrWhiteSpace(patameters.Gender))
            {
                patameters.Gender = patameters.Gender.Trim();
                var gender = Enum.Parse<Gender>(patameters.Gender);

                items = items.Where(x => x.Gender == gender);
            }

            if (!string.IsNullOrWhiteSpace(patameters.Q))
            {
                patameters.Q = patameters.Q.Trim();

                items = items.Where(x => x.EmployeeNo.Contains(patameters.Q) ||
                  x.FirstName.Contains(patameters.Q) ||
                  x.LastName.Contains(patameters.Q));
            }

            //if (!string.IsNullOrWhiteSpace(patameters.OrderBy))
            //{
            //    if (patameters.OrderBy.ToLowerInvariant()=="name")
            //    {
            //        items = items.OrderBy(x => x.FirstName).ThenBy(x => x.LastName);
            //    }
            //}

            var mappingDictionary = propertyMappingService.GetPropertyMapping<EmployeeResource, Employee>();

            items.ApplySort(patameters.OrderBy,mappingDictionary);

            return await items.ToListAsync();
        }
        public void UpdateEmployee(Employee employee)
        {
            // _myContext.Entry(employee).State = EntityState.Modified;  // 可以自动跟踪状态
        }
    }
}
