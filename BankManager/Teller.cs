namespace BankManager;

public class Teller
{
    private readonly AccountRepository _accountRepository;
    public Teller(AccountRepository accountRepository){
        _accountRepository = accountRepository;
    }

    public int CheckBalance()
    {
        return _accountRepository.CheckBalance();
    }

    public int ProcessTransaction(Transaction amount)
    {
        _accountRepository.ProcessTransaction(amount);
        return CheckBalance();
    }

}
