using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Helper
{
    public interface ICustomHash
    {
        public string GetHashPassword(string text);

        public string GetHashResetPassword(int id);


    }
}
