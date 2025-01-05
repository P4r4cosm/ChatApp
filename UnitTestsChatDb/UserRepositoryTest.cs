
using ChatDb;
namespace UnitTestsChatDb
{
    [TestClass]
    public class UserRepositoryTest
    {
        Random random = new Random();
        [TestMethod]
        public void CreateUserTest_SaveUserToDB()
        {
            //Arrange
            var valuepass = random.Next(10000, 1000000000).ToString();
            var password = PasswordManager.HashPassword(valuepass, out string salt);
            var UserRepository = new UserRepository(new ChatContext());
            var name = (UserRepository.GetAllUsersQuery().Max(u => u.Id)+1).ToString();
            //Act
            var user = new User
            {
                Name = $"TestUser{name}",
                Age = random.Next(10, 100),
                Password = password,
                Salt = salt
            };
            
            //Assert
            Assert.IsTrue(UserRepository.CreateUser(user));
        }
        [TestMethod]
        [ExpectedException(typeof(Microsoft.EntityFrameworkCore.DbUpdateException))]
        public void CreateUserTest_ReturnEx()
        {
            //Arrange
            var value = random.Next(10000, 1000000000).ToString();
            var password = PasswordManager.HashPassword(value, out string salt);

            //Act
            var user = new User
            {
                Name = "TestUser",
                Age = random.Next(10, 100),
                Password = password,
                Salt = salt
            };
            var UserRepository = new UserRepository(new ChatContext());
            //Assert
            UserRepository.CreateUser(user);
        }
    }
}
