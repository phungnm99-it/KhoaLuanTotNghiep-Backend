using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.ModelDTO
{
    public class SubscriberDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
    }
}
