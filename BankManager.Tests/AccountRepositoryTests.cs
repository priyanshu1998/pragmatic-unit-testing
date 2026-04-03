using Moq;

namespace BankManager.Tests
{
    [TestClass]
    public class AccountRepositoryTest : BaseTestClass
    {
        private AccountRepository _accountRepository = null!;

        [TestInitialize]
        public override void TestInit()
        {
            base.TestInit();
            _accountRepository = new AccountRepository();
        }

        [TestMethod]
        public void CheckBalance_RequestsTheAccountBalanceFromRepository()
        {
            var balance = _accountRepository.CheckBalance();

            Assert.AreEqual(0, balance, "The initial balance of an account should be zero.");
        }

        [TestMethod]
        public void ProcessTransaction_TransactionValueGiven_TellerSubmitsTransaction()
        {
            const int transactionAmount = 100;
            var simpleTransaction = new SimpleTransaction(transactionAmount);

            _accountRepository.ProcessTransaction(simpleTransaction);

            var balance = _accountRepository.CheckBalance();

            Assert.AreEqual(transactionAmount, balance,
                "After processing a simple transaction, the account balance should equal the transaction amount.");
        }
    }
}