﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcpClient.PublicClasses
{
    public class PublicChat
    {
        public PublicUser User1 { get; private set; }
        public PublicUser User2 { get; private set; }
        public List<PublicMessage> Messages { get; set; }
        
        public PublicChat(PublicUser user1, PublicUser user2, List<PublicMessage> messages)
        {
            User1 = user1;
            User2 = user2;
            Messages = messages;
        }
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (var message in Messages.AsEnumerable().Reverse().ToList())
            {
                result.AppendLine(message.ToString());
            }
            return result.ToString();
        }
        public PublicUser GetDifferentUser(PublicUser currentUser)
        {
            return User1.Equals(currentUser) ? User2 :User1;
        }
    }
}
