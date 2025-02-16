using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcpClient.PublicClasses;

namespace tcpClient.UI
{
    public static class PublicChatDisplayer
    {
        public static void DisplayUserChats(List<PublicChat> chatList, PublicUser currentUser)
        {
            int ChatIndex = 0;
            foreach (PublicChat chat in chatList)
            {
                var LastMessage = chat.Messages.FirstOrDefault();
                var sender = LastMessage?.Sender;
                var recepient = LastMessage?.Recipient;
                PublicUser differentUser = chat.GetDifferentUser(currentUser);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"\t {ChatIndex+1}) ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Чат с {differentUser}");
                Console.WriteLine($"Последнее сообщение:");
                ChatIndex++;
                Console.ForegroundColor = ConsoleColor.White;
                PrintMessageForUserInChat(LastMessage, currentUser);
            }
        }
        public static void DisplayChat(PublicChat chat, PublicUser currentUser)
        {
            var sender = chat.Messages.FirstOrDefault()?.Sender;
            var recepient = chat.Messages.FirstOrDefault()?.Sender;
            PublicUser differentUser = chat.GetDifferentUser(currentUser);
            foreach (var message in chat.Messages)
            {
                PrintMessageForUserInChat(message, currentUser);
            }
        }
        public static void PrintMessageForUserInChat(PublicMessage Message, PublicUser currentUser)
        {
            //текущий пользователь сообщения слева
            //собеседник сообщения справа
            Console.ForegroundColor = ConsoleColor.White;
            if (Message.Sender.Equals(currentUser))
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;
                var cursorPosition = Console.GetCursorPosition();
                Console.SetCursorPosition(Console.WindowLeft + 2, cursorPosition.Top);
                Console.Write($"{Message.Text}");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine();
                Console.WriteLine(Message.DepartureTime);

            }
            else
            {
                var cursorPosition = Console.GetCursorPosition();
                var messageLength = Math.Max(Message.Text.Length, Message.DepartureTime.ToString().Length);

                Console.BackgroundColor = ConsoleColor.DarkCyan;
                Console.SetCursorPosition(Console.WindowWidth - messageLength - 2, cursorPosition.Top);
                Console.Write($"{Message.Text}");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine();
                Console.SetCursorPosition(Console.WindowWidth - messageLength - 2, Console.GetCursorPosition().Top);
                Console.WriteLine($"{Message.DepartureTime}");
            }
        }
    }
}
