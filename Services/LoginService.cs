using BankApi.Data;
using BankApi.Data.DTOs;
using BankApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

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
            var hashedPassword = HashPassword(user.Pwd);
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == user.Email && u.Pwd == hashedPassword);    
        }

        public async Task<User?> Check_existence(UserDTOSignUp user)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == user.Email && u.Pwd == user.Pwd);
        }

        public async Task<User> SignUpAsync(UserDTOSignUp user)
        {
            var newUser = new User();

            newUser.Email = user.Email; 
            newUser.Pwd = user.Pwd;
            newUser.PhoneNumber= user.PhoneNumber;
            newUser.Name = user.Name;   

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }
        public async Task<UserDTOOut?> GetById(int id)
        {

            var user = await _context.Users.FindAsync(id);
            var userDTO = new UserDTOOut
            {
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };

            return userDTO;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

    }
}
