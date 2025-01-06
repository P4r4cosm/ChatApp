using ChatDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace tcpClient
{
    internal static class UserAuthorization
    {
        public static async Task<User> Login(SslStream sslStream)
        {
            while (true)
            {
                Console.Write("Введите логин: ");
                var login = Console.ReadLine();
                Console.Write("Введите пароль: ");
                var password = Console.ReadLine();
                await SecureCommunication.SendMessageToServer("Login", sslStream);
                await SecureCommunication.SendMessageToServer($"{login} {password}", sslStream);
                var loginResult =  await SecureCommunication.ReadServerMessage(sslStream);
                Console.WriteLine(loginResult);

                if (loginResult == "Login Successfull")
                {
                    var userData = await SecureCommunication.ReadServerMessage(sslStream);
                    var CurrentUser = JsonSerializer.Deserialize<User>(userData);
                    Console.WriteLine(CurrentUser.ToString());
                    return CurrentUser;
                }
            }
            
        }
        public static async Task<User> CreateAccount(SslStream sslStream)
        {
            while (true)
            {
                Console.Write("Введите логин: ");
                var login = Console.ReadLine();
                Console.Write("Введите пароль: ");
                var password = Console.ReadLine();
                int age;
                while (true)
                {
                    Console.Write("Введите возраст: ");
                    if (!int.TryParse(Console.ReadLine(), out age))
                    {
                        Console.WriteLine("Возраст не может быть преобразован в число");
                    }
                    else break;
                }


                await SecureCommunication.SendMessageToServer("CreateAccount", sslStream);
                await SecureCommunication.SendMessageToServer($"{login} {password} {age}", sslStream);
                var serverAnswer = await SecureCommunication.ReadServerMessage(sslStream);
                if (serverAnswer == "Error")
                {
                    Console.WriteLine(await SecureCommunication.ReadServerMessage(sslStream));
                }
                else
                {
                    var user = JsonSerializer.Deserialize<User>(await SecureCommunication.ReadServerMessage(sslStream));
                    return user;
                }
            }
        }
    }
}
