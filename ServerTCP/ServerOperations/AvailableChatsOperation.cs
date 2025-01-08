using ChatDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace ServerTCP.ServerOperations
{
    public class AvailableChatsOperation: AbstractOperationServer<bool>
    {
        public override string Name { get; } = "AvailableChats";
        public override Dictionary<string, object> Data { get; set; }
        public override Task<bool> Execute(SslStream stream, ChatContext database)
        {
            return Task.FromResult(true);
        }
    }
}
