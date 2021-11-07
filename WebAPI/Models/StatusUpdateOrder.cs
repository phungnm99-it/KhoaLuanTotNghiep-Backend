using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI.Models
{
    public partial class StatusUpdateOrder
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? UpdatedBy { get; set; }
        public string Detail { get; set; }
        public DateTime UpdatedTime { get; set; }

        public virtual Order Order { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
    }
}
