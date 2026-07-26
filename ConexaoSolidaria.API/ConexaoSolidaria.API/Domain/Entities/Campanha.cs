using Domain.DTOs;

namespace Domain.Entities
{
    public class Campanha : EntityBase
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public decimal MetaFinanceira { get; set; }
        public decimal ValorTotalArrecadado { get; set; }
        public StatusCampanha Status { get; set; }
        public DateTime DataCriacao { get; set; }
        public ICollection<Doacao> Doacoes { get; set; } = new List<Doacao>();
    }
}