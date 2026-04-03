using Moq;

namespace BankManager.Tests
{
    [TestClass]
    public abstract class BaseTestClass
    {
        [TestInitialize]
        public virtual void TestInit()
        {
            Logging.Logger = Mock.Of<ILogger>();
        }
    }
}