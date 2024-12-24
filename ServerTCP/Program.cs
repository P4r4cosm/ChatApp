using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
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
                        var buffer = new List<byte>();
                        int bytesRead;

                        // Читаем данные до символа '\n'
                        while ((bytesRead = stream.ReadByte()) != -1 && bytesRead != '\n')
                        {
                            buffer.Add((byte)bytesRead);
                        }

                        // Проверка на завершение соединения
                        if (bytesRead == -1)
                        {
                            Console.WriteLine("Клиент завершил соединение");
                            break;
                        }

                        // Обработка сообщения
                        var message = Encoding.UTF8.GetString(buffer.ToArray());
                        Console.WriteLine($"Сообщение от клиента {client.Client.RemoteEndPoint}: {message}");

                        // Ответ клиенту
                        var response = Encoding.UTF8.GetBytes("Сообщение получено\n");
                        await stream.WriteAsync(response);
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
