using ChatDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerTCP
{
    static public class AccountChecker
    {
        public static bool Verify(string login, string password, ChatContext db, out User user)
        {
            user = db.Users.FirstOrDefault(u=>u.Name == login);
            if (user == null) return false;
            if (PasswordManager.VerifyPassword(password, user.Password, user.Salt)) return true;
            return false;
        }
    }
}
