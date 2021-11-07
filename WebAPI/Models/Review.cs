using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI.Models
{
    public partial class Review
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? ProductId { get; set; }
        public string Content { get; set; }
        public DateTime ReviewTime { get; set; }

        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
    }
}
