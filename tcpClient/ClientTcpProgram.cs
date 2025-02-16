using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Net.Security;
using tcpClient.ClientOperations;
using tcpClient.PublicClasses;
using tcpClient.UI;
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
                #region login
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
                #endregion

                
                //Console.Clear(); //чистим вывод консоли
                var operation = new GetUserChatsOperation(sslStream);
                var publicChatList = await operation.RunOperation();
                //Console.Write("Чтобы перейти к чату введите его номер: "); 
                //var position = Console.GetCursorPosition(); //запоминаем позицию курсора
                //Console.WriteLine();
                //PublicChatDisplayer.DisplayUserChats(publicChatList, CurrentUser); //выводим все чаты
                //Console.SetCursorPosition(position.Left + 1, position.Top); //возвращаем курсор на прежнее место
                //string a = Console.ReadLine(); //считываем в нём клавишу
                //Console.Clear();
                //PublicChatDisplayer.DisplayChat(publicChatList[int.Parse(a)], CurrentUser);
                var manager = new PageManager();
                manager.PushPage(new ChatsPage(publicChatList, CurrentUser, manager));

                //var sobesednik = publicChatList[int.Parse(a)].GetDifferentUser(CurrentUser);
                //var message = CurrentUser.SendMessage("Zdarova blya", sobesednik);
                //var sendedMessage = new SendMessageOperation(sslStream, message).RunOperation();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при работе с SSL: {ex.Message}");
            }
        }

    }
}