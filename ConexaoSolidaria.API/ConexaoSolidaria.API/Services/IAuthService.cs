using Domain.DTOs;
using Domain.Entities;

namespace Services
{
    public interface IAuthService
    {
        Task<Usuario> RegisterAsync(RegistroUsuarioDto dto);
        Task<string> LoginAsync(LoginDto dto);
    }
}
