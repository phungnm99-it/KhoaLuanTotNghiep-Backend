using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.ModelDTO;

namespace WebAPI.Utils
{
    public interface IJwtUtils
    {
        public string GenerateToken(UserDTO user);
        public int? ValidateToken(string token);
    }
}
