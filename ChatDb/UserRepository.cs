using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatDb
{
    internal class UserRepository
    {
        private ChatContext db;
        public UserRepository(ChatContext db)
        {
            this.db = db;
        }
        public void CreateUser(User user)
        {
            try
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        public void UpdateUser(User user, string name)
        {
            try
            {
                user.Name = name;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void DeleteUser(User user)
        {
            try
            {
                db.Users.Remove(user);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public IQueryable<User> GetAllUsersQuery()
        {
            try
            {
                return db.Users.Select(u => u);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Enumerable.Empty<User>().AsQueryable();

            }
        }
    }
}
