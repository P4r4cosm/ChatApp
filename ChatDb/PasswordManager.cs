using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
namespace ChatDb
{
    public class PasswordManager
    {
        public static string HashPassword(string password, out string salt)
        {
            byte[] saltBytes = new byte[16];
            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(saltBytes);
            }
            salt = Convert.ToBase64String(saltBytes);

            using (var pdkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100_000))
            {
                byte[] hashBytes = pdkdf2.GetBytes(32);
                return Convert.ToBase64String(hashBytes);
            }
        }
        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(storedSalt);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100_000))
            {
                byte[] hashBytes = pbkdf2.GetBytes(32);
                string computedHash = Convert.ToBase64String(hashBytes);
                return computedHash == storedHash;
            }
        }
    }
}
