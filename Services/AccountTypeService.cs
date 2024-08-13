using BankApi.Data;
using BankApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BankApi.Services
{
    public class AccountTypeService
    {
        private readonly BankContext _context;

        public AccountTypeService(BankContext context) { 
        
            _context = context;

        }



        public async Task<IEnumerable<AccountType>> GetAccountTypes()
        {
            return await _context.AccountTypes.ToListAsync();
        }

        public async Task<AccountType> GetById(int id)
        {
            return await _context.AccountTypes.FindAsync(id);
        }

        public async Task<AccountType> Create(AccountType accountType)
        {

            _context.AccountTypes.Add(accountType);
            await _context.SaveChangesAsync();  
            return accountType; 

        }

    }
}
