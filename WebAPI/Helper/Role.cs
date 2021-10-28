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

        public const int SuperAdminRoleId = 1;
        public const int AdminRoleId = 2;
        public const int ShipperRoleId = 3;
        public const int UserRoleId = 4;
    }
}
