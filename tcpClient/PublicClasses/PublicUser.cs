using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcpClient.PublicClasses
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
        public PublicMessage SendMessage(string content, PublicUser recipient)
        {
            return new PublicMessage(content, this, recipient, false);
        }
        public override bool Equals(object? obj)
        {
            if (obj is not PublicUser other) return false;
            return Name == other.Name;
        }
    }
}
