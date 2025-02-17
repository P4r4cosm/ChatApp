using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcpClient.ClientOperations;
using tcpClient.PublicClasses;

namespace tcpClient.UI
{
    public class ConcreteChatPage : IPage
    {
        public PublicChat Chat { get; private set; }

        public PageManager PageManager { get; private set; }
        public PublicUser CurrentUser { get; private set; }
        public PublicUser DifferentUser { get; private set; }
        public ConcreteChatPage(PublicChat chat, PublicUser currentUser, PageManager manager)
        {
            Chat = chat;
            CurrentUser = currentUser;
            PageManager = manager;
            DifferentUser=Chat.GetDifferentUser(currentUser);
        }
        public void Display()
        {
            ConsoleExtensions.ClearFullScreen();
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"\t====Чат с пользователем {DifferentUser.Name}====");
            Console.BackgroundColor = ConsoleColor.Black;
            PublicChatDisplayer.DisplayChat(Chat, CurrentUser);
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\n\t0 для возвращения назад/выхода");
            Console.Write("\tВведите сообщение: ");
            Console.BackgroundColor = ConsoleColor.Black;

        }
        public async void HandleInput()
        {
            string input = Console.ReadLine();
            if (input == "0")
            {
                PageManager.PopPage();
                return;
            }
            var message =  new PublicMessage(text: input, sender: CurrentUser, 
                recipient: DifferentUser, isViewed: false);
            var op = await new SendMessageOperation(PageManager.SslStream, message).RunOperationAsync();
            Chat.Messages.Add(message);
            PageManager.pageStack.Pop();
            PageManager.PushPage(this);
        }
    }
}
