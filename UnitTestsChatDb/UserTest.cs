using ChatDb;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using User = ChatDb.User;
namespace UnitTestsChatDb
{
    [TestClass]
    public class UserTest
    {
        [TestMethod]
        public void SendMessageTest_ReturnMessage()
        {
            //Arrange
            var user = new User { Name = "Username", Age = 20 };
            var recipient = new User { Name = "Recipient", Age = 19 };
            //Act
            var message = user.SendMessage("content", recipient);
            //Assert
            Assert.AreEqual(new Message { Text = "content", Recipient = recipient, Sender = user }, message);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SendMessageTest_ReturnException()
        {
            //Arrange
            var user = new User { Name = "Username", Age = 20 };
            //Act

            var message = user.SendMessage("content", null);
        }
    }
}
