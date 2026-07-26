using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Security.Claims;

namespace ConexaoSolidaria.API.Controllers;

[ApiController]
[Route("api/doacoes")]
public class DoacaoController : ControllerBase
{
    private readonly IDoacaoService _service;

    public DoacaoController(IDoacaoService service)
    {
        _service = service;
    }

    [Authorize(Roles = "Doador")]
    [HttpPost]
    public async Task<IActionResult> Doar(
        CriarDoacaoDto dto)
    {
        var usuarioId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var id = await _service.CriarDoacaoAsync(
            usuarioId,
            dto);

        return Accepted(new
        {
            doacaoId = id,
            mensagem = "Doação recebida para processamento."
        });
    }
}