using BankApi.Data;
using BankApi.Data.DTOs;
using BankApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BankApi.Services
{
    public class LoginService
    {

        public BankContext _context; 
        public LoginService(BankContext context)
        {
            _context = context;
        }

        public async Task<User?> GetAdmin(UserDTO user)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == user.Email && u.Pwd == user.Pwd);    
        }


    }
}
