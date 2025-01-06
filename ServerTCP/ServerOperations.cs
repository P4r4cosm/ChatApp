using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerTCP
{
    public class ServerOperations
    {
        public static async Task SendMessageToClient(string message, SslStream stream)
        {
            var response = Encoding.UTF8.GetBytes($"{message}\n");
            await stream.WriteAsync(response);
        }
        public static async Task<string> ReadClientMessage(SslStream stream, TcpClient client)
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
    }
}
