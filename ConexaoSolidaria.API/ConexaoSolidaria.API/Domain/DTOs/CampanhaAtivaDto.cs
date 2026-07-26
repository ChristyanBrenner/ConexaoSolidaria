namespace Domain.DTOs
{
    public class CampanhaAtivaDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public decimal MetaFinanceira { get; set; }
        public decimal ValorTotalArrecadado { get; set; }
    }
}