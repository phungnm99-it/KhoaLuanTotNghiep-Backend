using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Helper
{
    public static class Role
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string Shipper = "Shipper";
        public const string Admin = "Admin";
        public const string User = "User";
        public const string Admins = "SuperAdmin, Admin";
    }
}
