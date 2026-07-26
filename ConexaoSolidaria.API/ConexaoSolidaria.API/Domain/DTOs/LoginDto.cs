using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs
{
    public class LoginDto
    {
        [Required, EmailAddress] public string Email { get; set; }
        [Required] public string Senha { get; set; }
    }
}
