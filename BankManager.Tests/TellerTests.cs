namespace BankManager.Tests;

[TestClass]
public sealed class TellerTests
{
    private static Teller _teller = null!;

    [ClassInitialize]
    public static void ClassInit(TestContext context) { }


    [TestMethod]
    public void CheckBalance_WithNoTransactions_Returns0Balance()
    {
        var teller = new Teller();

        var balance = teller.CheckBalance();

        const int expectedBalance = 0;
        Assert.AreEqual(expectedBalance, balance, "Empty account should have a 0 balance.");
    }

    [TestMethod]
    public void ProcessTransaction_WithOneDeposit_ReturnsBalanceEqualToDeposit()
    {
        var teller = new Teller();

        var depositAmount = new SimpleTransaction(100);
        var balance = teller.ProcessTransaction(depositAmount);

        Assert.AreEqual(depositAmount.CalculateTotalTransaction(), balance, "Balance should be equal to the single deposit amount.");
    }
}
