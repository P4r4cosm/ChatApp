using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcpClient.UI
{
    public interface IPage
    {
        public void Display();
        public void HandleInput();
    }
}
