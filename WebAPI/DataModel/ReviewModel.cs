using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.DataModel
{
    public class ReviewModel
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
    }
}
