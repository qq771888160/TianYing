using System;
using System.Collections.Generic;
using System.Text;

namespace TianYing.Infrasturcture.Resources
{
    public class CompanyAddResource
    {
        public string Name { get; set; }
        public string Introduction { get; set; }


        public ICollection<EmployeeAddResource> Employees { get; set; } = new List<EmployeeAddResource>();
    }
}
