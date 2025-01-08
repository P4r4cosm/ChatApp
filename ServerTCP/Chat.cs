using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatDb;
using Npgsql.EntityFrameworkCore.PostgreSQL.ValueGeneration.Internal;
namespace ServerTCP
{
    public class Chat
    {
        public User User1 { get; private set; }
        public User User2 { get; private set; }
        public List<Message> Messages { get; set; }
        public MessageRepository MessageRepository { get; private set; }
        public IEnumerable<Message> GiveLastPageMessages(int PageSize = 10)
        {
            var sortedMessages = Messages.OrderByDescending(m => m.DepartureTime);
            return sortedMessages.Take(PageSize);
        }

        public Chat(User user1, User user2, MessageRepository messageRepository)
        {
            User1 = user1;
            User2 = user2;
            MessageRepository = messageRepository;
            Messages = messageRepository.GetMessagesBetweenUsers(User1, User2).
                OrderByDescending(m => m.DepartureTime).Take(100).ToList();
        }
        public override string ToString() 
        {
            StringBuilder result = new StringBuilder();
            foreach (var message in Messages.AsEnumerable().Reverse().ToList())
            {
                result.AppendLine(message.ToString());
            }
            return result.ToString();
        }
    }
}
