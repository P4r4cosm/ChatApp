using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using ChatDb;
using System.Net.Security;

class ClientTcpProgram
{
    static public User CurrentUser { get; set; }

    static async Task SendMessageToServer(string message, SslStream sslStream)
    {
        var response = Encoding.UTF8.GetBytes($"{message}\n");
        await sslStream.WriteAsync(response);
    }

    static async Task<string> ReadServerMessage(SslStream sslStream)
    {
        var buffer = new List<byte>();
        int bytesRead;

        // Читаем данные до символа '\n'
        while ((bytesRead = sslStream.ReadByte()) != -1 && bytesRead != '\n')
        {
            buffer.Add((byte)bytesRead);
        }

        // Обработка сообщения
        var message = Encoding.UTF8.GetString(buffer.ToArray());
        return message;
    }
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

            while (true)
            {
                Console.Write("Введите логин: ");
                var login = Console.ReadLine();
                Console.Write("Введите пароль: ");
                var password = Console.ReadLine();
                await SendMessageToServer($"{login} {password}", sslStream);

                var loginResult = await ReadServerMessage(sslStream);
                Console.WriteLine(loginResult);

                if (loginResult == "Login Succesfull")
                {
                    var userData = await ReadServerMessage(sslStream);
                    CurrentUser = JsonSerializer.Deserialize<User>(userData);
                    Console.WriteLine(CurrentUser.ToString());
                    break;
                }
            }

            while (true)
            {
                var message = CurrentUser.SendMessage("Привет Настя",
                    new User
                    {
                        Id = 4,
                        Name = "Nastysha",
                        Age = 20,
                        Password = "t7uRBuLxhoIxBRT/HqsodWVvMeS3D72y8NZdk3PeVO4=",
                        Salt = "L8QeGR+DjLin1L2fXt5n+Q=="
                    });

                await SendMessageToServer(JsonSerializer.Serialize(message), sslStream);
                await Task.Delay(1000);
                break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при работе с SSL: {ex.Message}");
        }
    }

}
