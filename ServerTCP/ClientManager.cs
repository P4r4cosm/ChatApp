using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace ServerTCP
{
    public static class ClientManager
    {
        public static ConcurrentDictionary<int, SslStream> ActiveClients {  get; private set; } = new();

        public static void AddClient(int userId, SslStream stream)
        {
            ActiveClients.TryAdd(userId, stream);
        }

        public static void RemoveClient(int userId)
        {
            ActiveClients.TryRemove(userId, out _);
        }

        public static SslStream GetClientStream(int userId)
        {
            ActiveClients.TryGetValue(userId, out var stream);
            return stream;
        }
    }
}
