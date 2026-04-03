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


        [TestMethod]
        [DataRow(0)]
        [DataRow(10)]
        [DataRow(-1)]
        public void GetBalances_WithOneTransaction_ReturnsTotalOfTransactions(int transactionAmount)
        {
            var transaction = new SimpleTransaction(transactionAmount);

            _accountRepository.ProcessTransaction(transaction);

            var totalOfTransactions = transaction.CalculateTotalTransaction();
            var currentBalance = _accountRepository.CheckBalance();


            Assert.AreEqual(totalOfTransactions, currentBalance,
                "After processing a simple transaction, the account balance should equal the transaction amount.");
        }


        public static IEnumerable<Transaction> GetTransactionAmounts()
        {
            yield return new SimpleTransaction (0);
            yield return new SimpleTransaction (10);
            yield return new SimpleTransaction (-1);

            yield return new FeeTransaction (100, 5);
        }


        [TestMethod]
        [DynamicData(nameof(GetTransactionAmounts))]
        public void GetBalances_WithOneTransaction_ReturnsTotalOfTransactions_From_MethodSource(Transaction transaction)
        {
            _accountRepository.ProcessTransaction(transaction);

            var totalOfTransactions = transaction.CalculateTotalTransaction();
            var currentBalance = _accountRepository.CheckBalance();


            Assert.AreEqual(totalOfTransactions, currentBalance,
                "After processing a simple transaction, the account balance should equal the transaction amount.");
        }



    }
}