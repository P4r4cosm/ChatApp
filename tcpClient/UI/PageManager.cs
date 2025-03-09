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
    public class PageManager
    {
        public Stack<IPage> pageStack = new Stack<IPage>();
        public List<PublicChat> Chats { get; private set; }
        public SslStream SslStream { get; private set; }
        public PageManager(SslStream sslStream, List<PublicChat> publicChats)
        {
            SslStream = sslStream;
            Chats = publicChats;
            OperationFactory.MessageRecived += OnMessageRecived;
        }
        public void PushPage(IPage page)
        {
            pageStack.Push(page);
            ConsoleExtensions.ClearFullScreen();
            page.Display();
            page.HandleInput();
        }
        public void PopPage()
        {
            if (pageStack.Count > 0)
            {
                pageStack.Pop();
                ConsoleExtensions.ClearFullScreen();
                var page = pageStack.Peek();
                page.Display();
                page.HandleInput();
            }
        }
        public void Refresh()
        {
            var currentPage = pageStack.Peek();
            pageStack.Pop();
            PushPage(currentPage);
        }


        public void OnMessageRecived(PublicMessage message)
        {
            var user1 = message.Sender;
            var user2 = message.Recipient;
            foreach (var chat in Chats)
            {
                if ((user1.Equals(chat.User1) && user2.Equals(chat.User2))
                    || (user1.Equals(chat.User2) && user2.Equals(chat.User1)))
                {
                    chat.Messages.Add(message);
                    chat.Messages = chat.Messages.OrderByDescending(x => x.DepartureTime).ToList();
                    //Refresh();
                    return;
                };

            }
        }
    }
}