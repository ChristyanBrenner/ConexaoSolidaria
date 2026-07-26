using Domain.DTOs;
using Domain.Entities;
using Domain.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Repositories;

namespace Services
{
    public class DoacaoService : IDoacaoService
    {
        private readonly AppDbContext _ctx;
        private readonly IPublishEndpoint _publishEndpoint;

        public DoacaoService(AppDbContext ctx, IPublishEndpoint publishEndpoint)
        {
            _ctx = ctx;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<int> CriarDoacaoAsync(
            int usuarioId,
            CriarDoacaoDto dto)
        {
            if (dto.ValorDoacao <= 0)
                throw new ApplicationException(
                    "Informe um valor válido.");

            var usuario = await _ctx.Usuario
                .FirstOrDefaultAsync(x => x.Id == usuarioId);

            if (usuario == null)
                throw new ApplicationException(
                    "Usuário não encontrado.");

            var campanha = await _ctx.Campanha
                .FirstOrDefaultAsync(x => x.Id == dto.IdCampanha);

            if (campanha == null)
                throw new ApplicationException(
                    "Campanha não encontrada.");

            if (campanha.Status != StatusCampanha.Ativa)
                throw new ApplicationException(
                    "A campanha não está ativa.");

            if (campanha.DataFim < DateTime.UtcNow)
                throw new ApplicationException(
                    "A campanha já foi encerrada.");

            var doacao = new Doacao
            {
                UsuarioId = usuarioId,
                CampanhaId = dto.IdCampanha,
                ValorDoacao = dto.ValorDoacao,
                Status = StatusDoacao.Pendente,
                DataCriacao = DateTime.UtcNow
            };

            _ctx.Doacao.Add(doacao);
            await _ctx.SaveChangesAsync();

            var evento = new DoacaoRecebidaEvent
            {
                DoacaoId = doacao.Id,
                CampanhaId = doacao.CampanhaId,
                UsuarioId = doacao.UsuarioId,
                ValorDoacao = doacao.ValorDoacao,
                DataRecebimento = doacao.DataCriacao
            };

            await _publishEndpoint.Publish(evento);

            return doacao.Id;
        }
    }
}