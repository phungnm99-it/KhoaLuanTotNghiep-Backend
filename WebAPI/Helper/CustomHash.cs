using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Helper
{
    public class CustomHash : ICustomHash
    {
        private readonly HashSettings settings;
        public CustomHash(IOptions<HashSettings> hashPasswordSettings)
        {
            settings = hashPasswordSettings.Value;
        }
        public string GetHashPassword(string text)
        {
            return GetHash(text);
        }
        public string GetHashResetPassword(int id)
        {
            return GetHash(id.ToString()).Substring(0,32);
        }
        private string GetHash(string text)
        {
            using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text + settings.HashMixString));
                // Get the hashed string.  
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
