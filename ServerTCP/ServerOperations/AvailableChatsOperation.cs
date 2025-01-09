using ChatDb;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using tcpClient.PublicClasses;

namespace ServerTCP.ServerOperations
{
    public class AvailableChatsOperation:  UserAbstractOperation
    {
        public override string Name { get; } = "AvailableChats";
        public override User CurrentUser { get; protected set; }
        public AvailableChatsOperation(User currentUser)
        {
            CurrentUser = currentUser;
        }
        public override async Task Execute(SslStream stream, ChatContext database)
        {
            var ChatRepository = new ChatRepository(database);
            var chats = ChatRepository.GetAllUserChats(CurrentUser);
            var publicChatList = new List<PublicChat>();
            foreach (var chat in chats) publicChatList.Add(PublicConverter.Convert(chat));
            var response = new
            {
                responseAnswer = Name,
                data = publicChatList,
            };
            await SecureCommunication.SendMessageToClient(JsonSerializer.Serialize(response), stream);
        }
    }
}
