using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TianYing.Infrasturcture.Resources
{
    public class CompanyAddResource
    {
        [Display(Name ="公司名称")]
        [Required(ErrorMessage ="{0}是必填的")]
        [MaxLength(100,ErrorMessage ="{0}的最大长度为{1}")]
        public string Name { get; set; }
        [Display(Name ="简介")]
        [StringLength(500, MinimumLength =10,ErrorMessage = "{0}长度范围从{2}到{1}")]
        public string Introduction { get; set; }


        public ICollection<EmployeeAddResource> Employees { get; set; } = new List<EmployeeAddResource>();
    }
}
