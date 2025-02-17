using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using tcpClient.ClientOperations;
using tcpClient.PublicClasses;

namespace tcpClient.UI
{
    public class SearchUserPage: IPage
    {
        public PageManager PageManager { get; private set; }
        public PublicUser CurrentUser { get; private set; }
        public SslStream SslStream { get; private set; }
        public SearchUserPage(SslStream sslStream, PageManager pageManager, PublicUser user)
        {
            PageManager = pageManager;
            CurrentUser = user;
            SslStream = sslStream;
        }
        public void Display()
        {
            ConsoleExtensions.ClearFullScreen();
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\n\t 0. Назад (выход)");
            Console.Write("\tВведите имя интересующего пользователя: ");
            Console.BackgroundColor = ConsoleColor.Black;
        }
        public void HandleInput()
        {
            PublicUser user = null;
            while (user == null)
            {
                string input = Console.ReadLine();
                var op = new FindUserOperation(SslStream);
                op.Data["UserName"] = input;
                user = op.RunOperationAsync().Result;
            }
            PageManager.pageStack.Pop();
            var chatsPage = PageManager.pageStack.Peek() as ChatsPage;
            var chats = chatsPage.Chats;
            foreach (var item in chats)
            {
                Console.WriteLine(item.User1.ToString()+"\t"+item.User2) ;
                if ((item.User1.Equals(user) && item.User2.Equals(CurrentUser)) || (item.User2.Equals(user) && item.User1.Equals(CurrentUser)))
                {
                    var page = new ConcreteChatPage(item,CurrentUser, PageManager);
                    PageManager.PushPage(page);
                }

            }
            var chat = new PublicChat(CurrentUser, user, new List<PublicMessage>());
            chatsPage.Chats.Add(chat);
            PageManager.PushPage(new ConcreteChatPage(chat, CurrentUser, PageManager));
        }
    }
}
