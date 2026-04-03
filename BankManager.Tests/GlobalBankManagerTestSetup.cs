using Moq;

namespace BankManager.Tests
{
    [TestClass]
    public class GlobalBankManagerTestSetup
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            Logging.Logger = Mock.Of<ILogger>();
        }
    }
}