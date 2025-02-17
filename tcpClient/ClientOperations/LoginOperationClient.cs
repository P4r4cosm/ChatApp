
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
    public class LoginOperationClient : AbstractOperationClient
    {
        public override string Name { get; protected set; } 
        public override Dictionary<string, object> Data { get; }
        public override SslStream SslStream { get; }
        public LoginOperationClient(SslStream sslStream, bool isExists)
        {
            SslStream = sslStream;
            Data = new Dictionary<string, object>();
            Name = isExists ? "Login" : "CreateAccount";
        }
        public override async Task<PublicUser> RunOperationAsync()
        {
            Data.Clear();
            Console.Write("Введите логин: ");
            var login = Console.ReadLine();
            Console.Write("Введите пароль: ");
            var password = Console.ReadLine();


            Data.Add("password", password);
            Data.Add($"login", login);
            var request = new
            {
                operation = Name,
                data = Data
            };
            await SecureCommunication.SendMessageToServer(JsonSerializer.Serialize(request), SslStream);
            var loginResult = JsonSerializer.Deserialize<Dictionary<string, object>>(await SecureCommunication.ReadServerMessage(SslStream));

            if (loginResult?["responseAnswer"].ToString()== $"{Name} OK")
            {
                Console.WriteLine(loginResult["responseAnswer"].ToString());
                var user = JsonSerializer.Deserialize<PublicUser>(loginResult["data"].ToString());
                return user;
            }
            else
            {
                Console.WriteLine(loginResult["responseAnswer"].ToString());
                Console.WriteLine(loginResult["data"].ToString());
            }

            return null;
        }
    }
}
