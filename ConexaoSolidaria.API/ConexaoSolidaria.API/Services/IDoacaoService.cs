using Domain.DTOs;

namespace Services
{
    public interface IDoacaoService
    {
        Task<int> CriarDoacaoAsync(
            int usuarioId,
            CriarDoacaoDto dto);
    }
}