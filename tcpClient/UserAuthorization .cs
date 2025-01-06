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
                await ClientOperations.SendMessageToServer("Login", sslStream);
                await ClientOperations.SendMessageToServer($"{login} {password}", sslStream);
                var loginResult =  await ClientOperations.ReadServerMessage(sslStream);
                Console.WriteLine(loginResult);

                if (loginResult == "Login Successfull")
                {
                    var userData = await ClientOperations.ReadServerMessage(sslStream);
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
                    Console.Write("Введите возраст");
                    if (!int.TryParse(Console.ReadLine(), out age))
                    {
                        Console.WriteLine("Возраст не может быть преобразован в число");
                    }
                    else break;
                }


                await ClientOperations.SendMessageToServer("CreateAccount", sslStream);
                await ClientOperations.SendMessageToServer($"{login} {password} {age}", sslStream);
                var serverAnswer = await ClientOperations.ReadServerMessage(sslStream);
                if (serverAnswer == "Error")
                {
                    Console.WriteLine(await ClientOperations.ReadServerMessage(sslStream));
                }
                else
                {
                    var user = JsonSerializer.Deserialize<User>(await ClientOperations.ReadServerMessage(sslStream));
                    return user;
                }
            }
        }
    }
}
