using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

class ServerTcpProgram
{

    static async void SendMessageToClient(string message, NetworkStream stream)
    {
        var response = Encoding.UTF8.GetBytes($"{message}\n");
        await stream.WriteAsync(response);
    }
    static string ReadClientMessage(NetworkStream stream, TcpClient client)
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
    static async Task Main()
    {
        var server = new TcpListener(IPEndPoint.Parse("127.0.0.1:8080"));
        try
        {
            server.Start();
            Console.WriteLine("Server is running");

            while (true)
            {
                using var client = await server.AcceptTcpClientAsync();
                Console.WriteLine($"Входящее подключение: {client.Client.RemoteEndPoint}");
                var stream = client.GetStream();

                try
                {
                    while (true)
                    {
                        var login = ReadClientMessage(stream, client);
                        if (login == "Paracosm")
                        {
                            SendMessageToClient("Login Succesfull", stream);
                            break;
                        }
                        else
                        {
                            SendMessageToClient($"Login Failed", stream);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при обработке клиента: {ex.Message}");
                }
            }
        }
        finally
        {
            server.Stop();
        }
    }
}
