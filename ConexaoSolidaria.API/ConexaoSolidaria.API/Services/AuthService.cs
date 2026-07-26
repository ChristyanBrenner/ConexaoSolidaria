using Domain.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _ctx;
        private readonly JwtSettings _jwt;
        private readonly PasswordHasher<Usuario> _hasher;

        public AuthService(AppDbContext ctx, IOptions<JwtSettings> jwtOptions)
        {
            _ctx = ctx;
            _jwt = jwtOptions.Value;
            _hasher = new PasswordHasher<Usuario>();
        }

        public async Task<Usuario> RegisterAsync(RegistroUsuarioDto dto)
        {
            if (_ctx.Usuario.Any(u => u.Email == dto.Email))
                throw new ApplicationException("Email já cadastrado.");

            if(!ValidarCpf(dto.Cpf))
                throw new ApplicationException("Informe um CPF válido.");

            if (!ValidarSenha(dto.Senha))
                throw new ApplicationException("Senha não atende requisitos de segurança.");

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Cpf = dto.Cpf,
                Email = dto.Email,
                SenhaHash = dto.Senha,
                DataCriacao = DateTime.Now
            };

            usuario.SenhaHash = _hasher.HashPassword(usuario, dto.Senha);
            _ctx.Usuario.Add(usuario);
            await _ctx.SaveChangesAsync();

            return usuario;
        }       

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var Usuario = _ctx.Usuario.SingleOrDefault(u => u.Email == dto.Email);

            if (Usuario == null)
                throw new ApplicationException("Credenciais inválidas.");

            var res = _hasher.VerifyHashedPassword(Usuario, Usuario.SenhaHash, dto.Senha);

            if (res == PasswordVerificationResult.Failed)
                throw new ApplicationException("Credenciais inválidas.");

            return GenerateToken(Usuario);
        }       

        private string GenerateToken(Usuario Usuario)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, Usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, Usuario.Email),
            new Claim(ClaimTypes.Name, Usuario.Nome),
            new Claim(ClaimTypes.Role, Usuario.Role)
        };

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.ExpireMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private bool ValidarSenha(string pwd)
        {
            if (pwd.Length < 8) return false;
            bool hasUpper = pwd.Any(char.IsUpper);
            bool hasLower = pwd.Any(char.IsLower);
            bool hasDigit = pwd.Any(char.IsDigit);
            bool hasSpecial = pwd.Any(ch => !char.IsLetterOrDigit(ch));
            return hasUpper && hasLower && hasDigit && hasSpecial;
        }
        private bool ValidarCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            if (cpf.Length != 11)
                return false;

            if (new string(cpf[0], cpf.Length) == cpf)
                return false;

            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);

            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;

            resto = resto < 2 ? 0 : 11 - resto;

            string digito = resto.ToString();

            tempCpf += digito;

            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;

            resto = resto < 2 ? 0 : 11 - resto;

            digito += resto.ToString();

            return cpf.EndsWith(digito);
        }
    }
}
