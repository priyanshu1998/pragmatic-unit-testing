namespace BankManager.Tests;

[TestClass]
public sealed class TellerTests
{
    [TestMethod]
    public void CheckBalance_WithNoTransactions_Returns0Balance()
    {
        var teller = new Teller();

        var balance = teller.CheckBalance();

        const int expectedBalance = 0;
        Assert.AreEqual(expectedBalance, balance, "Empty account should have a 0 balance.");
    }
}
