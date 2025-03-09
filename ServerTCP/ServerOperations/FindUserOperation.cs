using ChatDb;
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
    public class FindUserOperation : UserAbstractOperation
    {
        public override string Name { get; } = "FindUserOperation";
        public override Dictionary<string, object> Data { get; set; }
        public override User CurrentUser { get; protected set; }
        public FindUserOperation(User currentUser)
        {
            CurrentUser = currentUser;
        }
        public override async Task Execute(SslStream stream, ChatContext database)
        {
            var userRep = new UserRepository(database);
            var username = Data["UserName"].ToString();
            var user = userRep.FindByName(username);
            if (user != null)
            {
                var pubUser = PublicConverter.Convert(user);
                var response = new
                {
                    responseAnswer = $"{Name} OK",
                    data = pubUser,
                };
                await SecureCommunication.SendMessageToClientAsync(JsonSerializer.Serialize(response), stream);
                return;
            }
            else
            {
                var response = new
                {
                    responseAnswer = $"{Name} Failed",
                    data = "Login or password is incorrect",
                };
                await SecureCommunication.SendMessageToClientAsync(JsonSerializer.Serialize(response), stream);
            }
        }
    }
}
