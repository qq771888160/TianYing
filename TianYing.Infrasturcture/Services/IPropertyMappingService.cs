using System;
using System.Collections.Generic;
using System.Text;

namespace TianYing.Infrasturcture.Services
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
    }
}
