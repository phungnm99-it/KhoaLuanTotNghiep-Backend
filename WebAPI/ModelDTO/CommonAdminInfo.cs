using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.ModelDTO
{
    public class CommonAdminInfo
    {
        public int TotalAccount { get; set; }
        public int TotalProduct { get; set; }
        public int TotalOrder { get; set; }
        public int TotalFeedback { get; set; }
    }
}
