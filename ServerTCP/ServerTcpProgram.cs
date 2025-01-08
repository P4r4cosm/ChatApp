using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using ChatDb;
using ServerTCP;
using ServerTCP.ServerOperations;
using System.Net.Security;
using ServerTCP.OperationFactories;
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

            User CurrentUser = null;
            OperationFactory.RegisterOperation<User>("Login", typeof(LoginOperationServer));
            OperationFactory.RegisterOperation<User>("CreateAccount", typeof(CreateAccountOperation));

            while (CurrentUser == null)
            {
                var operation = OperationFactory.CreateOperation<User>(await SecureCommunication.ReadClientMessage(sslStream));
                CurrentUser = await operation.Execute(sslStream, database);
            }
            Console.WriteLine($"Клиент авторизован: {CurrentUser.ToString()}");




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


            var messageRepostory = new MessageRepository(database);
            var userRepository = new UserRepository(database);
            var paracosm = userRepository.GetAllUsersQuery().Where(u => u.Name == "Paracosm").FirstOrDefault(); //получаем пользователя
            var nastysha = userRepository.GetAllUsersQuery().Where(u => u.Name == "Nastysha").FirstOrDefault();
            var message = paracosm.SendMessage("Hello World", nastysha); //создаём сообщение

            messageRepostory.CreateMessage(message); //сохраняем в базу
            var chat = new Chat(paracosm, nastysha, messageRepostory);
            Console.WriteLine(chat.ToString());



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

    public void Method()
    {
        throw new System.NotImplementedException();
    }
}
