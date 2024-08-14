using BankApi.Models;

namespace BankApi.Data.DTOs
{
    public class UserDTOSignUp
    {
        public string Name { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Pwd { get; set; } = null!;
        public string Confirm_Pwd { get; set; } = null!;
    }
}
