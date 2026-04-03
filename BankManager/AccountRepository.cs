namespace BankManager
{
    public class AccountRepository
    {
        readonly List<Transaction> _transactions = [];

        public virtual int CheckBalance()
        {
            return _transactions.Sum(t => t.CalculateTotalTransaction());   
        }

        public virtual void ProcessTransaction(Transaction amount)
        {
            _transactions.Add(amount);
        }
    }
}