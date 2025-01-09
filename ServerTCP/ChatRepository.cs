using ChatDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerTCP
{
    public class ChatRepository
    {
        public MessageRepository MessageRepository { get; private set; }
        public UserRepository UserRepository { get; private set; }
        public ChatRepository(ChatContext database) 
        {
            MessageRepository= new MessageRepository(database);
            UserRepository= new UserRepository(database);
        }
        public List<Chat> GetAllUserChats(User user)
        {
            var chats = MessageRepository.GetAllChatsUserNameQuery(user);
            var ChatList = new List<Chat>();
            foreach (var chat in chats)
            {
                var chatuser = UserRepository.FindByName(chat);
                ChatList.Add(new Chat(user, chatuser, MessageRepository));
            }
            return ChatList;
        }
    }
}
