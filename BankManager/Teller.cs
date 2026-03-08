namespace BankManager;

public class Teller
{
    readonly List<Transaction> _transactions = [];
    public int CheckBalance()
    {
        return _transactions.Sum(t => t.CalculateTotalTransaction());
    }

    public int ProcessTransaction(Transaction amount)
    {
        _transactions.Add(amount);
        return CheckBalance();
    }

}
