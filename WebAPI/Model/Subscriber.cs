using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI.Model
{
    public partial class Subscriber
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool? Status { get; set; }
    }
}
