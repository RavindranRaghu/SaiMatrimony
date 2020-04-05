using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SaiMatrimony.Auth
{
    public class Hashing
    {
        public string HashingPlain(string text)
        {
            string finalHashed = string.Empty;
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(text));

                StringBuilder hashed = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    hashed.Append(bytes[i].ToString("x2"));
                }
                finalHashed = hashed.ToString();
            }

            return finalHashed;
        }
    }
}
