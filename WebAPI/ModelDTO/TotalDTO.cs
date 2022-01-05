using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.ModelDTO
{
    public class TotalDTO
    {
        public string[] Month { get; set; }
        public decimal[] Money { get; set; }

        public TotalDTO(string[] a, decimal[] b)
        {
            Month = a;
            Money = b;
        }
    }
}
