using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankManager.Tests;

[TestClass]
public class FeeTransactionTests : TransactionTests

{
    public override Transaction GetTransactionWith(int baseAmount)
    {
        return GetTransactionWith(baseAmount, 0);
    }

    public Transaction GetTransactionWith(int baseAmount, int fee)
    {
        return new FeeTransaction(baseAmount, fee);
    }

    [TestMethod]
    public void CalculateTotalTransaction_AmountAndFeeProvided_ReturnsAmountMinusFee()
    {
        const int baseAmount = 100;
        const int fee = 5;
        var feeTransaction = new FeeTransaction(baseAmount, fee);

        var totalTransaction = feeTransaction.CalculateTotalTransaction();

        const int expectedTotalTransaction = baseAmount - fee;

        Assert.AreEqual(expectedTotalTransaction, totalTransaction,
            "Calculated transaction should equal the base amount minus the fee.");
    }
}
