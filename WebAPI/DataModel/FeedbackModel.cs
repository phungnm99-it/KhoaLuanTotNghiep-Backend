using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.DataModel
{
    public class FeedbackModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Topic { get; set; }
        public string Content { get; set; }
    }
}
