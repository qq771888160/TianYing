using System;
using System.Collections.Generic;
using System.Text;
using TianYing.Core.Entities;

namespace TianYing.Infrasturcture.Resources
{
   public class EmployeeAddResource
    {
        
        public string EmployeeNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }

        public DateTime DateofBirth { get; set; }
 
    }
}
