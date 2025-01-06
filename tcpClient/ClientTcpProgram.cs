using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using ChatDb;
using System.Net.Security;
using tcpClient.ClientOperations;
namespace tcpClient
{
    class ClientTcpProgram
    {
        static public User CurrentUser { get; set; }
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

                Console.WriteLine("Выберите вариант входа: \n " +
                 "1) Вход в существующий аккаунт \n" +
                 " 2) Создать новый аккаунт");

                var LoginOption = Console.ReadLine();
                var login = new LoginOperationClient(sslStream);
                while (CurrentUser == null)///логин / создание аккаунта
                {
                    switch (LoginOption)
                    {
                        case "1":
                            CurrentUser = await login.RunOperation();
                            Console.WriteLine(CurrentUser.ToString());
                            break;
                        case "2":
                            //CurrentUser = await userauthorization.createaccount(sslstream);
                            break;
                        default:
                            Console.WriteLine($"вариант {LoginOption} не поддерживается");
                            break;
                    }
                }
                while(true)
                {

                }
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