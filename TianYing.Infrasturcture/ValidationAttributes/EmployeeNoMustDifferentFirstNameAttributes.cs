using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TianYing.Infrasturcture.Resources;

namespace TianYing.Infrasturcture.ValidationAttributes
{
    class EmployeeNoMustDifferentFirstNameAttributes : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var addResource = (EmployeeAddResource)validationContext.ObjectInstance;

            if (addResource.EmployeeNo == addResource.FirstName)
            {
                return new ValidationResult("员工编号不可以和名相等", new[] { nameof(EmployeeAddResource) });
            }

            return ValidationResult.Success;
        }
    }
}
