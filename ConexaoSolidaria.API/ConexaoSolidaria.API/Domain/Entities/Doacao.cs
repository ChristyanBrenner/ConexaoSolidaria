using Domain.DTOs;

namespace Domain.Entities
{
    public class Doacao : EntityBase
    {
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
        public int CampanhaId { get; set; }
        public Campanha Campanha { get; set; } = null!;
        public decimal ValorDoacao { get; set; }
        public StatusDoacao Status { get; set; }
        public DateTime? DataProcessamento { get; set; }
    }
}