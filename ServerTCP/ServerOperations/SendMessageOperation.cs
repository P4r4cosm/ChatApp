using ChatDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using tcpClient.PublicClasses;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ServerTCP.ServerOperations
{
    public class SendMessageOperation : UserAbstractOperation
    {
        public override string Name { get; } = "SendMessage";
        public override User CurrentUser { get; protected set; }
        public override Dictionary<string, object> Data { get; set; }
        public SendMessageOperation(User currentUser)
        {
            CurrentUser = currentUser;
        }
        public override async Task Execute(SslStream stream, ChatContext database)
        {
            var publicMessage = JsonSerializer.Deserialize<PublicMessage>(Data["Message"].ToString());
            var userRepository = new UserRepository(database);
            var messageRepository = new MessageRepository(database);
            var sender = userRepository.FindByName(publicMessage.Sender.Name);
            var recipient = userRepository.FindByName(publicMessage.Recipient.Name);
            var message = new Message
            {
                Text = publicMessage.Text,
                DepartureTime = publicMessage.DepartureTime,
                Sender = sender,
                Recipient = recipient,
                IsViewed = publicMessage.IsViewed,
            };
            try
            {
                messageRepository.CreateMessage(message);
                var response = new
                {
                    responseAnswer = $"{Name} OK",
                    data = publicMessage,
                };
                await SecureCommunication.SendMessageToClient(JsonSerializer.Serialize(response), stream);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
                var response = new
                {
                    responseAnswer = $"{Name} Failed",
                    data = $"{ex.Message}",
                };
                await SecureCommunication.SendMessageToClient(JsonSerializer.Serialize(response), stream);
            }
        }
    }
}
