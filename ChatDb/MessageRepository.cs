using ChatDb.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatDb
{
    internal class MessageRepository
    {
        private ChatContext db;
        public MessageRepository(ChatContext db)
        {
            this.db = db;
        }
        public void CreateMessage(Message message)
        {
            if (message is null) throw new ArgumentNullException("message");
            try
            {
                db.Messages.Add(message);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        public void UpdateMessage(Message message, string content)
        {
            try
            {
                message.Text = content;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void DeleteMessage(Message message)
        {
            try
            {
                db.Messages.Remove(message);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public IQueryable<Message> GetAllMessagesQuery()
        {
            try
            {
                return db.Messages.Select(m => m);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Enumerable.Empty<Message>().AsQueryable();
            }
        }
    }
}
