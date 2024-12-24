using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning_Patterns
{
    class User
    {
        private static int id_counter = 0;
        public int Id { get; private set; }
        public string Name { get; set; }
        public User(string name)
        {
            Name = name;
            Id = id_counter++;
        }
        public override string ToString()
        {
            return $"user_id: {Id} - {Name}";
        }
    }
}
