using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcpClient.PublicClasses;
using ChatDb;
using System.ComponentModel.DataAnnotations;
namespace ServerTCP
{
    public static class PublicConverter
    {
        static public PublicUser Convert(User user)
        {
            return new PublicUser(user.Name, user.Age);
        }
        static public PublicMessage Convert(Message message)
        {
            var sender = Convert(message.Sender);
            var recipient = Convert(message.Recipient);
            return new PublicMessage(text:message.Text, sender: sender, recipient: recipient, 
                departureTime:message.DepartureTime, isViewed: message.IsViewed);
        }
        static public PublicChat Convert(Chat chat)
        {
            var user1 = Convert(chat.User1);
            var user2 = Convert(chat.User2);
            return new PublicChat(user1, user2);
        }
    }
}
