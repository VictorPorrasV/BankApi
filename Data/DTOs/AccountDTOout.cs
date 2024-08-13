namespace BankApi.Data.DTOs
{
    public class AccountDTOout
    {

        public int Id { get; set; }

        public String? AccountName { get; set; }

        public String? ClientName { get; set; }

        public decimal Balance { get; set; }
        public DateTime RegDate { get; set; }

    }
}
