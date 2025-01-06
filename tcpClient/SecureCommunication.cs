using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace tcpClient
{
    public static class SecureCommunication
    {
        public static async Task SendMessageToServer(string message, SslStream sslStream)
        {
            var response = Encoding.UTF8.GetBytes($"{message}\n");
            await sslStream.WriteAsync(response);
        }

        public static async Task<string> ReadServerMessage(SslStream sslStream)
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
    }
}
