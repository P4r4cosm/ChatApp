using ChatDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ChatDb;
using System.ComponentModel.DataAnnotations;
namespace ServerTCP.ServerOperations
{
    internal class CreateAccountOperation : AbstractOperationServer
    {
        public override string Name { get; } = "CreateAccount";
        public override async Task<User> Execute(SslStream SslStream, Dictionary<string, object> data,
            ChatContext Database)
        {
            var name = data["login"].ToString();
            var password = data ["password"].ToString();
            var cryptedPassword = PasswordManager.HashPassword(password, out string salt);
            var user = new User { Name = name, Password = cryptedPassword, Salt = salt};

            var userRepository= new UserRepository(Database);
            try
            {
                if (userRepository.CreateUser(user))
                {
                    var response = new
                    {
                        responseAnswer = $"{Name} OK",
                        data = UserToPublicUserConverter.Convert(user),
                    };
                    await SecureCommunication.SendMessageToClient(JsonSerializer.Serialize(response), SslStream);
                    return user;
                }
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
                var response = new
                {
                    responseAnswer = $"{Name} Failed",
                    data = $"{ex.Message}",
                };
                await SecureCommunication.SendMessageToClient(JsonSerializer.Serialize(response), SslStream);
                return null;
            }
            return null;
        }
    }
}
