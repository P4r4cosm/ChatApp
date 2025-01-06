using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace ServerTCP.ServerOperations
{
    public abstract class AbstractOperationClient
    {
        public abstract string Name { get; }
        public abstract Task RunOperation();
        public abstract Dictionary<string, object> Data { get; }
        public abstract SslStream SslStream { get; }

    }
}
