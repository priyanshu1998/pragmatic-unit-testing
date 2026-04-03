namespace BankManager;

public class Teller
{
    private readonly AccountRepository _accountRepository;
    public Teller(AccountRepository accountRepository){
        _accountRepository = accountRepository;
    }

    public int CheckBalance()
    {
        Logging.WriteLine("Checking the user's balance");
        return _accountRepository.CheckBalance();
    }

    public int ProcessTransaction(Transaction amount)
    {
        Logging.WriteLine("Processing a transaction of $" + amount.CalculateTotalTransaction());
        _accountRepository.ProcessTransaction(amount);
        return CheckBalance();
    }

}
