using ChatDb.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatDb
{
    public class MessageRepository
    {
        private ChatContext db;
        public MessageRepository(ChatContext db)
        {
            this.db = db;
        }
        public void CreateMessage(Message message)
        {
            if (message is null) throw new ArgumentNullException("message is null");
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
        public IQueryable<Message> GetMessagesBetweenUsers(User user1, User user2) //возвращаем все сообщения между двумя пользователями
        {
            try
            {
                return db.Messages.Where(m => (m.Sender == user1 && m.Recipient == user2)|| 
                (m.Sender == user2 && m.Recipient == user1));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Enumerable.Empty<Message>().AsQueryable();
            }
        }
        public List<string> GetAllChatsUserNameQuery(User user)
        {
            try
            {
                return db.Messages.Where(m=>m.Sender==user || m.Recipient==user)
                   .GroupBy(m => m.Sender.Name == user.Name ? m.Recipient.Name : m.Sender.Name).
                   Select(g => g.Key).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
