using Moq;

namespace BankManager.Tests
{
    [TestClass]
    public abstract class BaseTestClass
    {
        [TestInitialize]
        public virtual void TestInit()
        {
            var logger = Mock.Of<ILogger>();
            Mock.Get(logger).Setup(x => x.WriteLine(It.IsAny<string>()))
                .Callback<string>(Console.WriteLine);

            Logging.Logger = logger;
        }
    }
}