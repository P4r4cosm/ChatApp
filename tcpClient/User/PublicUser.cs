using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcpClient.User
{
    public class PublicUser
    {
        public string Name { get; set; }
        public int? Age { get; set; }
        public PublicUser(string name, int? age) 
        {
            Name= name;
            Age= age;
        }
        public override string ToString()
        {
            return $"User : {Name} Age: {Age}";
        }
        public async Task SendMessage(string content, PublicUser recipient)
        {
            Console.WriteLine("Отправлено сообщение :D");
        }
    }
}
