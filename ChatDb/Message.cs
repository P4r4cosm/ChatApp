using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatDb
{
    internal class Message
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        //Отправитель
        public int SenderId { get; set; }
        public User Sender { get; set; } = null!;  //навигационное свойство
        //Получатель
        public int RecipientId { get; set; }
        public User Recipient { get; set;} = null!; //навигационное свойство

        public override string ToString()
        {
            return $"{Text}\r\n\tОтправлено (кем): {Sender.Name}\t (кому): {Recipient.Name}";
        }
    }
}
