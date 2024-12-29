
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace ChatDb
{
    class ChatDbProgram
    {
        static void Main(string[] args)
        {
            #region bd
            using (var db = new ChatContext())
            {
                bool isAvailable = db.Database.CanConnect();
                Console.WriteLine(isAvailable ? "Успешное подключение к базе" : "Не удалось подключиться");
                var messageRepository = new MessageRepository(db);
                var userRepositoty = new UserRepository(db);
                #region addusers
                //var paracosmHash = PasswordManager.HashPassword("61323", out string salt);
                //Console.WriteLine(paracosmHash + "   "+ salt);
                //Console.WriteLine(PasswordManager.VerifyPassword("61323", paracosmHash, salt));
                //userRepositoty.CreateUser(new User { Name = "Paracosm", Age = 20, Password=paracosmHash, Salt=salt});


                //var nastyahash = PasswordManager.HashPassword("1234", out string nastyaSalt);
                //Console.WriteLine(nastyahash + "   " + nastyaSalt);
                //Console.WriteLine(PasswordManager.VerifyPassword("1234", nastyahash, nastyaSalt));
                //userRepositoty.CreateUser(new User { Name = "Nastysha", Age = 20, Password = nastyahash, Salt = nastyaSalt });
                #endregion

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
            #endregion
        }
    }
}