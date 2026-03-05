namespace BankManager.Tests;

[TestClass]
public abstract class TransactionTests
{
    public abstract Transaction GetTransactionWith(int baseAmount);

    [TestMethod]
    public void BaseAmount_AmountPassedToConstructor_ReturnsSameAmount()
    {
        const int nonZeroAmount = 5;
        var transaction = GetTransactionWith(nonZeroAmount);

        Assert.AreEqual(nonZeroAmount, transaction.BaseAmount,
            "The base amount of a transaction should be the same as the amount passed into the constructor.");

    }
}
