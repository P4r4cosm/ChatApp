using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcpClient.User;
using ChatDb;
namespace ServerTCP
{
    public static class UserToPublicUserConverter
    {
        static public PublicUser Convert(User user)
        {
            return new PublicUser(user.Name, user.Age);
        }
    }
}
