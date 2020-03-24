using System;
using System.Collections.Generic;
using System.Text;

namespace TianYing.Core.ResourceParameters
{
 public   class CompanyResourceParameter
    {
        private const int MaxPageSize = 20;
        public string CompanyName { get; set; }
        public string  SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        private int _pageSize=5;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize =( value>MaxPageSize)?MaxPageSize:value;
        }
    }
}
