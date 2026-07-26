namespace ConexaoSolidaria.Worker.Entities
{
    public class Doacao
    {
        public int Id { get; set; }
        public int CampanhaId { get; set; }
        public decimal ValorDoacao { get; set; }
        public StatusDoacao Status { get; set; }
        public DateTime? DataProcessamento { get; set; }
    }
    public enum StatusDoacao
    {
        Pendente = 1,
        Processada = 2,
        Rejeitada = 3
    }
}