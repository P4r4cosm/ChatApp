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
    public class GetUserChatsOperation : AbstractOperationClient
    {
        public override string Name { get; protected set; } = "GetUserChats";

        public override Dictionary<string, object> Data { get; }

        public override SslStream SslStream { get; }
        public GetUserChatsOperation(SslStream sslStream)
        {
            SslStream = sslStream;
            Data = new Dictionary<string, object>();
        }

        public async override Task<List<PublicChat>> RunOperationAsync()
        {
            var request = new
            {
                operation = Name,
                data = Data
            };
            await SecureCommunication.SendMessageToServer(JsonSerializer.Serialize(request), SslStream);
            var serverAnswer = JsonSerializer.Deserialize<Dictionary<string, object>>
                    (await SecureCommunication.ReadServerMessage(SslStream));
            if (serverAnswer["responseAnswer"].ToString() == $"{Name} OK")
            {
                return JsonSerializer.Deserialize<List<PublicChat>>(serverAnswer["data"].ToString());
            }
            else
            {
                Console.WriteLine(serverAnswer["responseAnswer"].ToString());
                Console.WriteLine(serverAnswer["data"].ToString());
            }
            return null;
        }
    }
}