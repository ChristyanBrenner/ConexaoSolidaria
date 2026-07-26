namespace Domain.DTOs
{
    public class AtualizarCampanhaDto
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public decimal MetaFinanceira { get; set; }
    }
}