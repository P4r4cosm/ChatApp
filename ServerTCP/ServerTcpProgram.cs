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
            while (true)
            {
                var loginOption = await ServerOperations.ReadClientMessage(sslStream, client);
                var accountLoginner = new AccountLoginner(client, database, sslStream);
                if (await accountLoginner.Authorization(loginOption)) break;
            }

            // чтение сообщения
            while (true)
            {
                var message = JsonSerializer.Deserialize<Message>(ServerOperations.ReadClientMessage(sslStream, client).Result);
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
