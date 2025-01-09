using ChatDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace ServerTCP.ServerOperations
{
    public abstract class UserAbstractOperation
    {
        public abstract string Name { get; }
        public abstract User CurrentUser { get; protected set; }
        public abstract Task Execute(SslStream stream, ChatContext database);
    }
}
