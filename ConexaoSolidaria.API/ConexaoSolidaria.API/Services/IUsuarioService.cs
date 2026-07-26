using Domain.DTOs;
using Domain.Entities;

namespace Services
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario> GetByIdAsync(int id);
        Task<Usuario> CreateAsync(RegistroUsuarioDto dto);
        Task<Usuario> UpdateAsync(int id, RegistroUsuarioDto dto);
        Task DeleteAsync(int id);
    }
}
