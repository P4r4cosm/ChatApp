using ChatDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using tcpClient.User;
namespace ServerTCP.ServerOperations
{
    internal class LoginOperationServer : AbstractOperationServer<User>
    {
        public override string Name { get; } = "Login";
        public override Dictionary<string, object> Data { get;  set; }
        public override async Task<User> Execute(SslStream SslStream, ChatContext Database)
        {
            if (AccountChecker.Verify(Data["login"].ToString(), Data["password"].ToString(), Database, out User user))
            {
                var response = new
                {
                    responseAnswer = $"{Name} OK",
                    data = UserToPublicUserConverter.Convert(user),
                };
                await SecureCommunication.SendMessageToClient(JsonSerializer.Serialize(response), SslStream);
                return user;
            }
            else
            {
                var response = new
                {
                    responseAnswer = $"{Name} Failed",
                    data = "Login or password is incorrect",
                };
                await SecureCommunication.SendMessageToClient(JsonSerializer.Serialize(response), SslStream);
                return null;
            }
        }
    }
}
