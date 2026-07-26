namespace Domain.Events
{
    public class DoacaoRecebidaEvent
    {
        public int DoacaoId { get; set; }
        public int CampanhaId { get; set; }
        public int UsuarioId { get; set; }
        public decimal ValorDoacao { get; set; }
        public DateTime DataRecebimento { get; set; }
    }
}
