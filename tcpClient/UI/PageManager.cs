using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcpClient.UI
{
    public class PageManager
    {
        public Stack<IPage> pageStack = new Stack<IPage>();
        public void PushPage(IPage page)
        {
            pageStack.Push(page);
            Console.Clear();
            page.Display();
            page.HandleInput();
        }
        public void PopPage()
        {
            if (pageStack.Count > 1)
            {
                pageStack.Pop();
                
                Console.Clear();
                var page = pageStack.Peek();
                page.Display();
                page.HandleInput();
            }
        }
    }
}
