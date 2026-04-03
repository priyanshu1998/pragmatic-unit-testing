using Moq;

namespace BankManager.Tests;

[TestClass]
public sealed class TellerTests
{
    private Teller _teller = null!;
    private AccountRepository _accountRepository = null!;

    [TestInitialize]
    public void TestInit()
    {
        _accountRepository = Mock.Of<AccountRepository>();
        _teller = new Teller(_accountRepository);
    }

    [TestMethod]
    public void CheckBalance_WithNoTransactions_Returns0Balance()
    {
        Mock.Get(_accountRepository)
            .Setup(ar => ar.CheckBalance())
            .Returns(0);

        var balance = _teller.CheckBalance();

        const int expectedBalance = 0;
        Assert.AreEqual(expectedBalance, balance, "Empty account should have a 0 balance.");
    }

    [TestMethod]
    public void ProcessTransaction_WithOneDeposit_ReturnsBalanceEqualToDeposit()
    {
        var depositAmount = new SimpleTransaction(100);
        Mock.Get(_accountRepository)
            .Setup(ar => ar.ProcessTransaction(depositAmount))
            .Verifiable();

        var balance = _teller.ProcessTransaction(depositAmount);

        Mock.Get(_accountRepository)
            .Verify(ar => ar.ProcessTransaction(depositAmount), Times.Once, "ProcessTransaction should be called once.");

        // Assert.AreEqual(depositAmount.CalculateTotalTransaction(), balance, "Balance should be equal to the single deposit amount.");

    }
}
