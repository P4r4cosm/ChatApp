using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using ChatDb;
using ServerTCP;
using System.Net.Security;
class ServerTcpProgram
{

    static async Task SendMessageToClient(string message, SslStream stream)
    {
        var response = Encoding.UTF8.GetBytes($"{message}\n");
        await stream.WriteAsync(response);
    }
    static async Task<string> ReadClientMessage(SslStream stream, TcpClient client)
    {
        var buffer = new List<byte>();
        int bytesRead;

        // Читаем данные до символа '\n'
        while ((bytesRead = stream.ReadByte()) != -1 && bytesRead != '\n')
        {
            buffer.Add((byte)bytesRead);
        }
        // Обработка сообщения
        var message = Encoding.UTF8.GetString(buffer.ToArray());
        return message;
    }
    static async Task HandleClientAsync(TcpClient client, ChatContext database, X509Certificate2 serverCertificate)
    {
        Console.WriteLine($"Входящее подключение: {client.Client.RemoteEndPoint}");
        using var sslStream = new SslStream(client.GetStream());

        try
        {
            // Аутентификация сервера
            await sslStream.AuthenticateAsServerAsync(serverCertificate, clientCertificateRequired: false,
                                                      checkCertificateRevocation: false);

            Console.WriteLine("SSL handshake completed.");
            while (true) // цикл авторизации
            {
                var authenticationString = await ReadClientMessage(sslStream, client);
                var login = authenticationString.Split(' ')[0];
                var password = authenticationString.Split(' ')[1];

                if (AccountChecker.Verify(login, password, database, out User user))
                {
                    await SendMessageToClient("Login Succesfull", sslStream);
                    await SendMessageToClient(JsonSerializer.Serialize(user), sslStream);
                    break;
                }
                else
                {
                    await SendMessageToClient("Incorrect login", sslStream);
                }
            }
            while (true)
            {
                var message = JsonSerializer.Deserialize<Message>(ReadClientMessage(sslStream, client).Result);
                Console.WriteLine(message);
                
                break;
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при обработке клиента: {ex.Message}");
        }
        finally
        {
            client.Close();
        }
    }
    static async Task Main()
    {
        var server = new TcpListener(IPEndPoint.Parse("127.0.0.1:8080"));
        try
        {
            server.Start();
            Console.WriteLine("Server is running");
            var database = new ChatContext();
            bool isAvailable = database.Database.CanConnect();
            Console.WriteLine(isAvailable ? "Успешное подключение к базе" : "Не удалось подключиться");

            var config = JsonDocument.Parse(File.ReadAllText("appsettings.json"));
            var certPath = config.RootElement.GetProperty("Certificate").GetProperty("Path").GetString();
            var certPassword = config.RootElement.GetProperty("Certificate").GetProperty("Password").GetString();
            var serverCertificate = new X509Certificate2(certPath, certPassword);

            while (true)
            {
                var client = await server.AcceptTcpClientAsync(); // Принимаем клиента
                _ = Task.Run(() => HandleClientAsync(client, database, serverCertificate)); // Запускаем обработку клиента в отдельной задаче
            }
        }
        finally
        {
            server.Stop();
        }
    }
}
