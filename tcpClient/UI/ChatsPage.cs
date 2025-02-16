﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcpClient.PublicClasses;

namespace tcpClient.UI
{
    internal class ChatsPage : IPage
    {
        public List<PublicChat> Chats { get; private set; }

        public PublicUser CurrentUser { get; private set; }
        public PageManager PageManager { get; private set; }
        public ChatsPage(List<PublicChat> chats, PublicUser user, PageManager pageManager)
        {
            Chats = chats;
            CurrentUser = user;
            PageManager = pageManager;
        }

        public void Display()
        {
            Console.Clear(); //чистим вывод консоли
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\t\t\t==== Список чатов ====");
            Console.BackgroundColor = ConsoleColor.Black;
            PublicChatDisplayer.DisplayUserChats(Chats, CurrentUser); //выводим все чаты
            Console.BackgroundColor=ConsoleColor.DarkRed;
            Console.WriteLine("\n\t 0. Назад (выход)");
            Console.Write("\t Выберите чат: ");
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
            if (int.TryParse(input, out int index) && index > 0 && index <= Chats.Count)
            {
                PageManager.PushPage(new ConcreteChatPage(Chats[index - 1],CurrentUser, PageManager));
            }
            else
            {
                Console.WriteLine("Неверный ввод. Попробуйте снова.");
            }
        }
    }
}
