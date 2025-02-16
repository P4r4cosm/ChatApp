using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcpClient.PublicClasses;

namespace tcpClient.UI
{
    public class ConcreteChatPage: IPage
    {
        public PublicChat Chat { get; private set; }

        public PageManager PageManager { get; private set; }
        public PublicUser CurrentUser { get; private set; }
        public ConcreteChatPage(PublicChat chat, PublicUser currentUser, PageManager manager)
        {
            Chat = chat;
            CurrentUser = currentUser;
            PageManager = manager;
        }
        public void Display()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"\t====Чат с пользователем {Chat.GetDifferentUser(CurrentUser).Name}====");
            Console.BackgroundColor = ConsoleColor.Black;
            PublicChatDisplayer.DisplayChat(Chat, CurrentUser);
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\n\t0 для возвращения назад/выхода");
            Console.Write("\tВведите сообщение: ");
            Console.BackgroundColor = ConsoleColor.Black;
            
        }
        public void HandleInput()
        {
            string input = Console.ReadLine();
            if (input == "0")
            {
                PageManager.PopPage();
                return;
            }
        }
    }
}
