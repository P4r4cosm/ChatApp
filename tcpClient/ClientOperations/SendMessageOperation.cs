using ServerTCP.ServerOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using tcpClient.PublicClasses;
namespace tcpClient.ClientOperations
{
    public class SendMessageOperation : AbstractOperationClient
    {
        public override string Name { get; protected set; } = "SendMessage";

        public override Dictionary<string, object> Data { get; }

        public override SslStream SslStream { get; }

        public PublicMessage Message { get; set; }

        public SendMessageOperation(SslStream sslStream, PublicMessage publicMessage)
        {
            SslStream = sslStream;
            Data = new Dictionary<string, object>();
            Message = publicMessage;
        }
        public override async Task<PublicMessage> RunOperationAsync()
        {
            Data.Clear();
            Data.Add("Message", Message);
            var request = new
            {
                operation = Name,
                data = Data,
            };
            await SecureCommunication.SendMessageToServer(JsonSerializer.Serialize(request), SslStream);
            var answer = JsonSerializer.Deserialize<Dictionary<string, object>>
                (await SecureCommunication.ReadServerMessage(SslStream));
            if (answer["responseAnswer"].ToString() == $"{Name} OK")
            {
                Console.WriteLine(answer["responseAnswer"].ToString());
                return JsonSerializer.Deserialize<PublicMessage>(answer["data"].ToString());
            }
            else
            {
                Console.WriteLine(answer["responseAnswer"].ToString());
            }
            return null;
        }
    }
}
