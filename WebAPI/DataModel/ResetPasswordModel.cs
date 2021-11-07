using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.DataModel
{
    public class ResetPasswordModel
    {
        public string HashId { get; set; }
        public string NewPassword { get; set; }
    }
}
