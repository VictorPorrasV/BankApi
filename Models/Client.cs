using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BankApi.Models;

public partial class Client
{
    public int Id { get; set; }
    [MaxLength(200,ErrorMessage = "El numero de telefono debe ser menor a 200 caracteres.")]
    public string Name { get; set; } = null!;

    [MaxLength(40,ErrorMessage ="El numero de telefono debe ser menor a 40 digitos")]    
    public string PhoneNumber { get; set; } = null!;
   
    
    [MaxLength(50, ErrorMessage = "El email  debe contener menos de 50 caracteres")]
    [EmailAddress(ErrorMessage ="El formato del emaril es incorrecto favor revisar")]
    public string? Email { get; set; }

    
    public DateTime RegDate { get; set; }

    [JsonIgnore]
    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
