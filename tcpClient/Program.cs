using System.Net.Sockets;
using System.Text;

class Program
{
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

        using var stream = tcpClient.GetStream();

        while (true)
        {
            Console.Write("Введите сообщение (END для завершения работы с сервером): ");
            var message = Console.ReadLine();

            if (message == "END")
            {
                Console.WriteLine("Завершение работы...");
                break;
            }

            // Отправка сообщения
            var data = Encoding.UTF8.GetBytes(message + "\n");
            await stream.WriteAsync(data);

            // Получение ответа
            var buffer = new List<byte>();
            int bytesRead;

            while ((bytesRead = stream.ReadByte()) != -1 && bytesRead != '\n')
            {
                buffer.Add((byte)bytesRead);
            }

            if (bytesRead == -1)
            {
                Console.WriteLine("Сервер закрыл соединение");
                break;
            }

            var response = Encoding.UTF8.GetString(buffer.ToArray());
            Console.WriteLine($"Ответ от сервера: {response}");
        }
    }
}
