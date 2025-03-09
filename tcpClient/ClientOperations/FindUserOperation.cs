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
    public class FindUserOperation:AbstractOperationClient
    {
        public override string Name { get; protected set; } = "FindUserOperation";

        public override Dictionary<string, object> Data { get; }

        public override SslStream SslStream { get; }
        public FindUserOperation(SslStream sslStream)
        {
            SslStream = sslStream;
            Data = new Dictionary<string, object>();
        }
        public async override Task RunOperationAsync()
        {
           
            var request = new
            {
                operation = Name,
                data = Data
            };
            await SecureCommunication.SendMessageToServerAsync(JsonSerializer.Serialize(request), SslStream);
            //var serverAnswer = JsonSerializer.Deserialize<Dictionary<string, object>>
            //        (await SecureCommunication.ReadServerMessageAsync(SslStream));
           
            //if (serverAnswer["responseAnswer"].ToString() == $"{Name} OK")
            //{
            //    return JsonSerializer.Deserialize<PublicUser>(serverAnswer["data"].ToString());
            //}
            //else
            //{
            //    Console.WriteLine(serverAnswer["responseAnswer"].ToString());
            //    Console.WriteLine("Пользователь не найден");
            //}
            //return null;
        }
    }
}
