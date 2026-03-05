namespace BankManager.Tests;

[TestClass]
public class SimpleTransactionTests : TransactionTests
{
    public override Transaction GetTransactionWith(int baseAmount)
    {
        return new SimpleTransaction(baseAmount);
    }

    [TestMethod]
    public void CalculateTotalTransaction_AmountProvided_ReturnsSameAmount()
    {
        const int baseAmount = 100;
        var simpleTransaction = new SimpleTransaction(baseAmount);

        var totalTransaction = simpleTransaction.CalculateTotalTransaction();

        Assert.AreEqual(baseAmount, totalTransaction,
            "Calculated transaction should equal the base amount.");
    }
}
 