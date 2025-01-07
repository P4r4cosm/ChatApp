﻿using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Net.Security;
using tcpClient.ClientOperations;
using tcpClient.User;
namespace tcpClient
{
    class ClientTcpProgram
    {
        static public PublicUser CurrentUser { get; set; }
        public static bool ValidateServerCertificate(object sender, X509Certificate certificate,
            X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        static async Task Main()
        {
            using TcpClient tcpClient = new TcpClient();
            Console.WriteLine("Клиент запущен");
            await tcpClient.ConnectAsync("127.0.0.1", 8080);

            if (tcpClient.Connected)
                Console.WriteLine($"Подключение с {tcpClient.Client.RemoteEndPoint} установлено");
            else
            {
                Console.WriteLine("Не удалось подключиться");
                return;
            }

            using SslStream sslStream = new SslStream(tcpClient.GetStream(), false,
                new RemoteCertificateValidationCallback(ValidateServerCertificate), null);

            try
            {
                // Аутентификация клиента
                await sslStream.AuthenticateAsClientAsync("localhost", null, checkCertificateRevocation: false);
                Console.WriteLine("SSL handshake completed.");
                
                bool isAccountExists = false;
                while (CurrentUser==null)
                {
                    var flag = true;
                    Console.WriteLine("Выберите вариант входа: \n" +
                         "1) Вход в существующий аккаунт \n" +
                         "2) Создать новый аккаунт");
                    while (flag)
                    {
                        var LoginOption = Console.ReadLine();
                        switch (LoginOption)
                        {
                            case "1":
                                isAccountExists = true;
                                flag = false;
                                break;
                            case "2":
                                isAccountExists = false;
                                flag = false;
                                break;
                            default:
                                Console.WriteLine("Неверное значение");
                                continue;
                        }
                    }
                    
                    var login = new LoginOperationClient(sslStream, isAccountExists);
                    while (true)
                    {
                       
                        CurrentUser = await login.RunOperation();
                        if (CurrentUser == null)
                        {
                            Console.WriteLine("Нажмите Esc чтобы вернуться назад или любую другую клавишу для продолжения.");
                            var key = Console.ReadKey(intercept: true); // Чтение клавиши без отображения символа
                            if (key.Key == ConsoleKey.Escape) // Проверка на Esc
                            {
                                break; // Выход из внутреннего цикла
                            }
                        }
                        else break;

                    }
                }
                
                Console.WriteLine(CurrentUser.ToString());
                //while (true)
                //{
                //    var message = CurrentUser.SendMessage("Привет Настя",
                //        new User
                //        {
                //            Id = 4,
                //            Name = "Nastysha",
                //            Age = 20,
                //            Password = "t7uRBuLxhoIxBRT/HqsodWVvMeS3D72y8NZdk3PeVO4=",
                //            Salt = "L8QeGR+DjLin1L2fXt5n+Q=="
                //        });

                //    await SecureCommunication.SendMessageToServer(JsonSerializer.Serialize(message), sslStream);
                //    await Task.Delay(1000);
                //    break;
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при работе с SSL: {ex.Message}");
            }
        }

    }
}