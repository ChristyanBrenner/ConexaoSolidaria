using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace ConexaoSolidaria.API.Controllers
{
    [ApiController]
    [Route("api/campanhas")]
    public class CampanhaController : ControllerBase
    {
        private readonly ICampanhaService _campanhaService;

        public CampanhaController(
            ICampanhaService campanhaService)
        {
            _campanhaService = campanhaService;
        }

        [Authorize(Roles = "GestorONG")]
        [HttpPost]
        public async Task<IActionResult> Criar(
            [FromBody] CriarCampanhaDto dto)
        {
            var id = await _campanhaService.CriarAsync(dto);

            return CreatedAtAction(
                nameof(ObterPorId),
                new { id },
                new
                {
                    campanhaId = id,
                    message = "Campanha criada com sucesso."
                });
        }

        [Authorize(Roles = "GestorONG")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Atualizar(
            int id,
            [FromBody] AtualizarCampanhaDto dto)
        {
            await _campanhaService.AtualizarAsync(id, dto);

            return Ok(new
            {
                message = "Campanha atualizada com sucesso."
            });
        }

        [Authorize(Roles = "GestorONG")]
        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> AlterarStatus(
            int id,
            [FromBody] AlterarStatusCampanhaDto dto)
        {
            await _campanhaService.AlterarStatusAsync(id, dto);

            return Ok(new
            {
                message = "Status alterado com sucesso."
            });
        }

        [AllowAnonymous]
        [HttpGet("ativas")]
        public async Task<IActionResult> ListarAtivas()
        {
            var campanhas =
                await _campanhaService.ListarAtivasAsync();

            return Ok(campanhas);
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var campanha =
                await _campanhaService.ObterPorIdAsync(id);

            if (campanha == null)
                return NotFound("Campanha não encontrada.");

            return Ok(campanha);
        }
    }
}