using BankApi.Data;
using BankApi.Data.DTOs;
using BankApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BankApi.Services
{
    public class AccountService
    {

        private readonly BankContext _context;  

        public AccountService(BankContext context)
        {

            _context = context; 
        }
        public async Task<IEnumerable<AccountDTOout>> GetAccounts()
        {


            return await _context.Accounts.Select(a => new AccountDTOout
            {
                Id = a.Id,
                AccountName = a.AccountTypeNavigation.Name,
                ClientName = a.Client.Name != null ? a.Client.Name : "",
                Balance = a.Balance,
                RegDate = a.RegDate
            }).ToListAsync();
        }



        public async Task<AccountDTOout?> GetDTObyID(int id)
        {


            return await _context.Accounts.Where(a=> a.Id == id).Select(a => new AccountDTOout
            {
                Id = a.Id,
                AccountName = a.AccountTypeNavigation.Name,
                ClientName = a.Client.Name != null ? a.Client.Name : "",
                Balance = a.Balance,
                RegDate = a.RegDate
            }).SingleOrDefaultAsync();
        }




        public async Task<Account?> GetById(int id)
        {
            return await _context.Accounts.FindAsync(id);
        }


        public async Task<Account> Create(AccountDTOin account)
        {
            var newAccount = new Account();

            newAccount.AccountType = account.AccountType;
            newAccount.ClientId = account.ClientId;
            newAccount.Balance= account.Balance;    


            _context.Accounts.Add(newAccount);
            await _context.SaveChangesAsync();
            return newAccount;
        }



        public async Task Update( AccountDTOin account)
        {


            var existingaccount = await GetById(account.Id);

            if (existingaccount is not null)
            {
                existingaccount.AccountType = account.AccountType;
                existingaccount.ClientId = account.ClientId;
                existingaccount.Balance = account.Balance;

                await _context.SaveChangesAsync();
            }

        }



        public async Task Delete(int id)
        {
            var AccountToDelete = await GetById(id);

            if (AccountToDelete is not null)
            {
                _context.Accounts.Remove(AccountToDelete);
                await _context.SaveChangesAsync();
            }




        }


        
    }
}
