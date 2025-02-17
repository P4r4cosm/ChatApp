using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcpClient.UI
{
    public static class ConsoleExtensions
    {
        public static void ClearFullScreen()
        {
            Console.Clear();
            Console.WriteLine("\x1b[3J");
        }
    }
}
