namespace Domain.Entities
{
    public class Usuario : EntityBase
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public string Role { get; set; } = "Doador";
        public ICollection<Doacao> Doacoes { get; set; } = new List<Doacao>();
    }
}