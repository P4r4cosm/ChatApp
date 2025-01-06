using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ChatDb;
namespace ServerTCP.ServerOperations
{
    abstract class AbstractOperationServer
    {
        public abstract string Name { get; }
        public abstract Task Execute(SslStream stream, Dictionary<string, object>data, ChatContext database);
    }
}
