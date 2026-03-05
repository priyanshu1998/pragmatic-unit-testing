namespace BankManager
{
    public abstract class Transaction
    {
        public int BaseAmount { get; private set; }

        protected Transaction(int baseAmount)
        {
            BaseAmount = baseAmount;
        }

        public abstract int CalculateTotalTransaction();

        public override string ToString()
        {
            return "Base = " + BaseAmount;
        }
    }
}