
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;
namespace ChatDb
{
    class ChatDbProgram
    {
        static void Main(string[] args)
        {
            #region bd
            using (var db = new ChatContext())
            {
                bool isAvailable = db.Database.CanConnect();
                Console.WriteLine(isAvailable ? "Успешное подключение к базе" : "Не удалось подключиться");
                var messageRepository = new MessageRepository(db);
                var userRepositoty = new UserRepository(db);
                var paracosm = db.Users.FirstOrDefault(u => u.Name == "Paracosm");
                var nastysha = db.Users.FirstOrDefault(u => u.Name == "Nastysha");

                var paracosmik = db.Users.FirstOrDefault(u => u.Name == "Paracosmik");
                messageRepository.CreateMessage(paracosmik.SendMessage($"Здарова еблан", paracosm));
                var random = new Random();
                //for (int i = 0; i < 20; i++)
                //{
                //    var item = random.Next();
                //    if (item % 3 == 0)
                //    {
                //        messageRepository.CreateMessage(paracosm.SendMessage($"text message № {i} {item}", nastysha));
                //    }
                //    else if (item%3 == 1)
                //    {
                //        messageRepository.CreateMessage(nastysha.SendMessage($"text message № {i} {item}", paracosm));
                //    }
                //    else
                //    {
                //        messageRepository.CreateMessage(paracosm.SendMessage($"text message № {i} {item}", paracosm));
                //    }
                //}

                var chats = messageRepository.GetAllChatsUserNameQuery(paracosm);
                foreach (var chat in chats)
                {
                    Console.WriteLine(chat);
                }
            }
            #endregion
        }
    }
}