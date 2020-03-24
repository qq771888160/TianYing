using System;
using System.Collections.Generic;
using System.Text;
using TianYing.Core.Entities;

namespace TianYing.Core.ResourceParameters
{
    public class EmployeeResourcePatameter
    {
        private const int MaxPage = 20;
        public string Gender { get; set; }
        public string Q { get; set; }
        public int PageNumber { get; set; } = 1;
        private int pageSize = 5;

        public int PageSize
        {
            get => pageSize = 5;
            set => pageSize = (value > MaxPage) ? MaxPage : value;
        }

        public string OrderBy { get; set; } = "Name";

    }
}
