using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TianYing.Core.Entities;
using TianYing.Infrasturcture.Resources;

namespace TianYing.Infrasturcture.Services
{
    public class PropertyMappingService:IPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> employeePropertyMapping = new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
            {"Id",new PropertyMappingValue(new List<string >{ "Id"} ) },
            {"CompanyId",new PropertyMappingValue(new List<string >{ "CompanyId" } ) },
            {"  EmployeeNo",new PropertyMappingValue(new List<string >{ "EmployeeNo" } ) },
            {"Name",new PropertyMappingValue(new List<string >{ "FirstName","LastName"} ) },
            {"GenderDisplay",new PropertyMappingValue(new List<string >{ "Gender"} ) },
              {"Age",new PropertyMappingValue(new List<string >{ "DateOfBirth"}, true) }
        };

        private IList<IPropertyMapping> propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            propertyMappings.Add(new PropertyMapping<EmployeeResource, Employee>(employeePropertyMapping));

        }
        public Dictionary<string ,PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            var matchingMapping = propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            var matchingMappings = matchingMapping.ToList();
            if (matchingMappings.Count==1)
            {
                return matchingMappings.First().MappingDictionary;
            }

            throw new Exception($"无法找到唯一映射关系：{typeof(TSource)} ,{typeof(TDestination)}");

        }
    }
}
