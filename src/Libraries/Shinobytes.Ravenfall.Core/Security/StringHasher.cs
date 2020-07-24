using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shinobytes.Ravenfall.Core.Security
{
    public class StringHasher
    {
        private static string HASHKEY = "ravenfall_yayayayayayya_temp_key";

        public static string Get(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            return ComputeSha256Hash(text + "|" + HASHKEY);
        }

        private static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            var builder = new StringBuilder();
            using (var sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string                   
                builder.AppendJoin("", bytes.Select(x => x.ToString("x2")));

                return builder.ToString();
            }
        }
    }
}
