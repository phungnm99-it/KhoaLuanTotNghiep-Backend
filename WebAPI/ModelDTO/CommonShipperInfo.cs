using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.ModelDTO
{
    public class CommonShipperInfo
    {
        public int WorkingDate { get; set; }
        public int DeliveredOrder { get; set; }
        public int TotalOrder { get; set; }
        public int DeliveringOrder { get; set; }

    }
}
