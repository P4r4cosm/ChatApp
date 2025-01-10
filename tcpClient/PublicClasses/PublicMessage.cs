using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcpClient.PublicClasses
{
    public class PublicMessage
    {
        public string Text { get; set; }
        //Отправитель
        public PublicUser Sender { get; set; }

        public PublicUser Recipient { get; set; }
        public DateTime DepartureTime { get; set; } = DateTime.UtcNow;

        public bool IsViewed { get; set; }

        public PublicMessage(string text, PublicUser sender, PublicUser recipient, DateTime departureTime, bool isViewed)
        {
            Text = text;
            Sender = sender;
            DepartureTime = departureTime;
            Recipient = recipient;
            IsViewed = isViewed;
        }
        public PublicMessage(string text, PublicUser sender, PublicUser recipient, bool isViewed)
        {
            Text = text;
            Sender = sender;
            Recipient = recipient;
            IsViewed = isViewed;
        }
        public PublicMessage() 
        {

        }

        public override string ToString()
        {
            return $"{Text}\r\n\tОтправлено (кем): {Sender.Name}\t (кому): {Recipient.Name}" +
                $" \t {DepartureTime.ToLocalTime()}";
        }
    }
}
