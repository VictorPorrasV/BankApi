using System;
using System.Collections.Generic;

namespace BankApi.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Pwd { get; set; }

    public string? AdminType { get; set; }

    public DateTime RegDate { get; set; }
}
