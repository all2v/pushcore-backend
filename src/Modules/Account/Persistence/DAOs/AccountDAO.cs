namespace Astrum.Account.DAOs;

public class AccountDAO
{
    private AccountDbContext _dbContext;

    public AccountDAO(AccountDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}