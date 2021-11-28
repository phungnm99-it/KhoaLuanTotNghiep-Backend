using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.DataModel
{
    public class SortModel
    {
        public int Page { get; set; }
        public string OrderType { get; set; }
        public string BrandName { get; set; }
    }
}
