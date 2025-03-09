using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerTCP
{
    public class SecureCommunication
    {
        private static SemaphoreSlim _sslSemaphore = new SemaphoreSlim(1, 1);
        public static async Task SendMessageToClientAsync(string message, SslStream stream)
        {
            //await _sslSemaphore.WaitAsync();
            try
            {
                var response = Encoding.UTF8.GetBytes($"{message}\n");
                await stream.WriteAsync(response);
            }
            finally
            {
                //_sslSemaphore.Release();
            }
        }
        public static async Task<string> ReadClientMessageAsync(SslStream stream)
        {
            //await _sslSemaphore.WaitAsync();
            try
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
            finally
            {
                //_sslSemaphore.Release();
            }
        }
    }
}
