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
        public static async Task SendMessageToServerAsync(string message, SslStream sslStream, CancellationToken token = default)
        {
            var response = Encoding.UTF8.GetBytes($"{message}\n");
            await sslStream.WriteAsync(response);
        }

        public static async Task<string> ReadServerMessageAsync(SslStream sslStream, CancellationToken token = default)
        {
            var buffer = new List<byte>();
            var oneByte = new byte[1];
            int bytesRead;

            while (!token.IsCancellationRequested)
            {
                bytesRead = await sslStream.ReadAsync(oneByte, 0, 1, token);
                if (bytesRead == 0 || oneByte[0] == (byte)'\n')
                    break;
                buffer.Add(oneByte[0]);
            }

            //token.ThrowIfCancellationRequested();

            return Encoding.UTF8.GetString(buffer.ToArray());
        }
    }
}
