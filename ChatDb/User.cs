﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ChatDb
{
    [Index("Name", IsUnique = true)]
    public class User
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = null!;
        public int? Age { get; set; }
        public string Password { get; set; } =null!;
        public string Salt { get; set; } = null!;
        public override string ToString()
        {
            return $"User {Id}: {Name} Age: {Age}";
        }
        public Message SendMessage(string content, User recipient)
        {
            if (recipient is null) throw new ArgumentNullException(nameof(recipient));
            return new Message { Text = content, Recipient=recipient, Sender=this};
        }
        public override bool Equals(object? obj)
        {
            if (obj is not User other) return false;
            return Id == other.Id && Name == other.Name;
        }
    }
}
