using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace ConexaoSolidaria.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistroUsuarioDto dto)
        {
            try
            {
                await _auth.RegisterAsync(dto);
                return Created("", new { message = "Usuário criado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var token = await _auth.LoginAsync(dto);
            return Ok(new { token });
        }
    }
}
