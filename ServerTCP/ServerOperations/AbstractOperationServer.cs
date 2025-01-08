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
    public abstract class AbstractOperationServer<T>
    {
        public abstract string Name { get; }
        public abstract Dictionary<string, object> Data { get;  set; }
        public abstract  Task<T> Execute(SslStream stream, ChatContext database);
    }
}
