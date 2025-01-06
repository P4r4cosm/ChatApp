﻿using ChatDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServerTCP
{
    public class AccountLoginner
    {
        private TcpClient Client { get; set; }

        private ChatContext Database { get; set; }

        private SslStream SslStream { get; set; }

        private UserRepository UserRepository { get; set; }
        public AccountLoginner(TcpClient client, ChatContext database, SslStream sslStream)
        {
            Client = client;
            Database = database;
            SslStream = sslStream;
            UserRepository = new UserRepository(Database);
        }
        public async Task<bool> Login()
        {
            while (true) // цикл авторизации
            {
                var authenticationString = await SecureCommunication.ReadClientMessage(SslStream);
                var login = authenticationString.Split(' ')[0];
                var password = authenticationString.Split(' ')[1];

                if (AccountChecker.Verify(login, password, Database, out User user))
                {
                    await SecureCommunication.SendMessageToClient("Login Successfull", SslStream);
                    await SecureCommunication.SendMessageToClient(JsonSerializer.Serialize(user), SslStream);
                    return true;
                }
                else
                {
                    await SecureCommunication.SendMessageToClient("Incorrect login", SslStream);
                    return false;
                }
            }
        }
        public async Task<bool> CreateAccount()
        {
            while (true)
            {
                var userString = await SecureCommunication.ReadClientMessage(SslStream);
                var name = userString.Split(" ")[0];
                var password = PasswordManager.HashPassword(userString.Split(" ")[1], out string salt);
                var age = int.Parse(userString.Split(' ')[2]);
                var user = new User { Name = name, Password = password, Salt = salt, Age = age };
                try
                {
                    if (UserRepository.CreateUser(user))
                    {
                        await SecureCommunication.SendMessageToClient("Account Created", SslStream);
                        await SecureCommunication.SendMessageToClient(JsonSerializer.Serialize(user), SslStream);
                        return true;
                    }
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
                {
                    Console.WriteLine(ex.Message);
                    await SecureCommunication.SendMessageToClient("Error", SslStream);
                    await SecureCommunication.SendMessageToClient(ex.Message, SslStream);
                    return false;
                }
            }
        }
        public async Task<bool> Authorization(string loginOption)
        {
            switch (loginOption)
            {
                case "Login":
                    return await Login() ? true : false;
                case "CreateAccount":
                    return await CreateAccount() ? true : false;
                default: return false;
            }
        }
    }
}
