using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TianYing.Core.Entities;
using TianYing.Infrasturcture.ValidationAttributes;

namespace TianYing.Infrasturcture.Resources
{

    [EmployeeNoMustDifferentFirstNameAttributes]
    public abstract class EmployeeAddOrUpdateResource : IValidatableObject
    {
        [Display(Name = "员工号")]
        [Required(ErrorMessage = "{0}是必填的")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "{0}的长度是{1}")]
        public  string EmployeeNo { get; set; }  // 子类特殊情况的话  重写这个属性 父类虚属性   public  abstract string EmployeeNo

        [Display(Name = "名")]
        [Required(ErrorMessage = "{0}是必填的")]
        [MaxLength(50, ErrorMessage = "{0}的最大长度不超过{1}")]
        public string FirstName { get; set; }

        [Display(Name = "姓"), Required(ErrorMessage = "{0}是必填的"), MaxLength(50, ErrorMessage = "{0}最大长度不超过{1}")]
        public string LastName { get; set; }
        [Display(Name = "性别")]
        public Gender Gender { get; set; }
        [Display(Name = "出生日期")]
        public DateTime DateofBirth { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FirstName == LastName)
            {
                //yield return new ValidationResult("姓和名不能一样", new[] { nameof(EmployeeAddResource) });
                yield return new ValidationResult("姓和名不能一样", new[] { nameof(FirstName), nameof(LastName) });
            }
        }
    }
