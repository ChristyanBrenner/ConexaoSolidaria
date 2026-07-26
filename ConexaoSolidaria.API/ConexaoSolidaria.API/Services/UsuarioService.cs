using Domain.DTOs;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;

namespace Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _context.Usuario
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Usuario> GetByIdAsync(int id)
        {
            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                throw new KeyNotFoundException("Usuário não encontrado.");

            return usuario;
        }
        public async Task<Usuario> CreateAsync(RegistroUsuarioDto dto)
        {
            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Cpf = dto.Cpf,
                Email = dto.Email,
                SenhaHash = dto.Senha,
            };

            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();

            return usuario;
        }
        public async Task<Usuario> UpdateAsync(int id, RegistroUsuarioDto dto)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
                throw new KeyNotFoundException("Usuário não encontrado.");

            usuario.Nome = dto.Nome;
            usuario.Email = dto.Email;
            usuario.SenhaHash = dto.Senha;

            await _context.SaveChangesAsync();
            return usuario;
        }
        public async Task DeleteAsync(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);

            if (usuario == null)
                throw new KeyNotFoundException("Usuário não encontrado.");

            _context.Usuario.Remove(usuario);
            await _context.SaveChangesAsync();
        }
    }
}
