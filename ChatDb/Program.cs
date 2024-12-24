
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace ChatDb
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new ChatContext())
            {
                bool isAvailable = db.Database.CanConnect();
                Console.WriteLine(isAvailable ? "Успешное подключение к базе" : "Не удалось подключиться");
                var messageRepository = new MessageRepository(db);
                var userRepositoty = new UserRepository(db);
                //userRepositoty.CreateUser(new User { Name = "Paracosm", Age = 20 });
                //userRepositoty.CreateUser(new User {Name="Nastya", Age=20 });
                var userlist = userRepositoty.GetAllUsersQuery();
                foreach (var user in userlist)
                { 
                    Console.WriteLine(user);
                }
                var paracosm = userlist.FirstOrDefault();
                //var message = paracosm?.SendMessage("Здарова, Настюха, с зачётом тебя!", userlist?.Where(u=>u.Name=="Nastya")?.FirstOrDefault());
                //messageRepository.CreateMessage(message);
                var messagelist = messageRepository.GetAllMessagesQuery();
                foreach (var message in messagelist)
                {
                    Console.WriteLine(message);
                }
            }
        }
    }
}