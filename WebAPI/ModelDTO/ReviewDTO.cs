﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.ModelDTO
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string UserName { get; set; }
        public string ImageUrl { get; set; }
        public string ProductName { get; set; }
        public string Content { get; set; }
        public DateTime ReviewTime { get; set; }
    }
}
