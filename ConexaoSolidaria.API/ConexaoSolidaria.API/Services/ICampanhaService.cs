using Domain.DTOs;
using Domain.Entities;

namespace Services
{
    public interface ICampanhaService
    {
        Task<int> CriarAsync(CriarCampanhaDto dto);
        Task AtualizarAsync(int id, AtualizarCampanhaDto dto);
        Task AlterarStatusAsync(int id, AlterarStatusCampanhaDto dto);
        Task<Campanha?> ObterPorIdAsync(int id);
        Task<List<CampanhaAtivaDto>> ListarAtivasAsync();
    }
}