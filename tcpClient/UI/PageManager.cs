using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace tcpClient.UI
{
    public class PageManager
    {
        public Stack<IPage> pageStack = new Stack<IPage>();

        public SslStream SslStream { get; private set; }
        public PageManager(SslStream sslStream)
        {
            SslStream = sslStream;
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
            if (pageStack.Count > 1)
            {
                pageStack.Pop();
                ConsoleExtensions.ClearFullScreen();
                var page = pageStack.Peek();
                page.Display();
                page.HandleInput();
            }
        }
    }
}