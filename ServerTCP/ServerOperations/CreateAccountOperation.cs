using ChatDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServerTCP.ServerOperations
{
    //internal class CreateAccountOperation: AbstractOperationServer
    //{
    //    public override ChatContext Database { get; }
    //    public override Dictionary<string, object> Data { get; }
    //    public override SslStream SslStream { get; }
    //    public override string Name { get; } = "Login";
    //    public CreateAccountOperation(ChatContext chatContext, TcpClient client, SslStream sslStream)
    //    {
    //        SslStream = sslStream;
    //        Data = new Dictionary<string, object>();
    //        Database = chatContext;
    //    }
    //    public override async Task<bool> RunOperation()
    //    {

    //        var authenticationString = await SecureCommunication.ReadClientMessage(SslStream, Client);
    //        var login = authenticationString.Split(' ')[0];
    //        var password = authenticationString.Split(' ')[1];

    //        if (AccountChecker.Verify(login, password, Database, out User user))
    //        {
    //            await SecureCommunication.SendMessageToClient("Login Successfull", SslStream);
    //            await SecureCommunication.SendMessageToClient(JsonSerializer.Serialize(user), SslStream);
    //            return true;
    //        }
    //        else
    //        {
    //            await SecureCommunication.SendMessageToClient("Incorrect login", SslStream);
    //            return false;
    //        }

    //    }
    //}
}
