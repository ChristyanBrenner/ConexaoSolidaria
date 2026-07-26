using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs
{
    public class RegistroUsuarioDto
    {
        [Required] public string Nome { get; set; }
        [Required] public string Cpf { get; set; }

        [Required, EmailAddress] public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Senha { get; set; }
    }
}