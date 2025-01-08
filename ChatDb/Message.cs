using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatDb
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        //Отправитель
        public int SenderId { get; set; }
        public User Sender { get; set; } = null!;  //навигационное свойство
        //Получатель
        public int RecipientId { get; set; }
        public User Recipient { get; set;} = null!; //навигационное свойство
        public DateTime DepartureTime { get; set; } = DateTime.UtcNow;

        public bool IsViewed { get; set; }

        public override string ToString()
        {
            return $"{Text}\r\n\tОтправлено (кем): {Sender.Name}\t (кому): {Recipient.Name}" +
                $" \t {DepartureTime.ToLocalTime()}";
        }
        public override bool Equals(object? obj)
        {
            if (obj is not Message other) return false;
            return Id == other.Id &&
                Text == other.Text &&
                Sender == other.Sender &&
                Recipient == other.Recipient;
        }
    }
}
