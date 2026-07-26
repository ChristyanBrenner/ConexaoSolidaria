using Domain.DTOs;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;

namespace Services
{
    public class CampanhaService : ICampanhaService
    {
        private readonly AppDbContext _ctx;

        public CampanhaService(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<int> CriarAsync(CriarCampanhaDto dto)
        {
            ValidarDadosCampanha(
                dto.Titulo,
                dto.Descricao,
                dto.DataInicio,
                dto.DataFim,
                dto.MetaFinanceira);

            var campanha = new Campanha
            {
                Titulo = dto.Titulo.Trim(),
                Descricao = dto.Descricao.Trim(),
                DataInicio = dto.DataInicio,
                DataFim = dto.DataFim,
                MetaFinanceira = dto.MetaFinanceira,
                ValorTotalArrecadado = 0,
                Status = StatusCampanha.Ativa,
                DataCriacao = DateTime.UtcNow
            };

            _ctx.Campanha.Add(campanha);
            await _ctx.SaveChangesAsync();

            return campanha.Id;
        }

        public async Task AtualizarAsync(
            int id,
            AtualizarCampanhaDto dto)
        {
            var campanha = await _ctx.Campanha
                .FirstOrDefaultAsync(x => x.Id == id);

            if (campanha == null)
                throw new KeyNotFoundException(
                    "Campanha não encontrada.");

            if (campanha.Status == StatusCampanha.Cancelada)
            {
                throw new ApplicationException(
                    "Não é possível editar uma campanha cancelada.");
            }

            ValidarDadosCampanha(
                dto.Titulo,
                dto.Descricao,
                dto.DataInicio,
                dto.DataFim,
                dto.MetaFinanceira);

            campanha.Titulo = dto.Titulo.Trim();
            campanha.Descricao = dto.Descricao.Trim();
            campanha.DataInicio = dto.DataInicio;
            campanha.DataFim = dto.DataFim;
            campanha.MetaFinanceira = dto.MetaFinanceira;

            await _ctx.SaveChangesAsync();
        }

        public async Task AlterarStatusAsync(
            int id,
            AlterarStatusCampanhaDto dto)
        {
            var campanha = await _ctx.Campanha
                .FirstOrDefaultAsync(x => x.Id == id);

            if (campanha == null)
                throw new KeyNotFoundException(
                    "Campanha não encontrada.");

            if (!Enum.IsDefined(typeof(StatusCampanha), dto.Status))
            {
                throw new ApplicationException(
                    "Status de campanha inválido.");
            }

            if (campanha.Status == StatusCampanha.Cancelada)
            {
                throw new ApplicationException(
                    "Uma campanha cancelada não pode ter seu status alterado.");
            }

            campanha.Status = dto.Status;

            await _ctx.SaveChangesAsync();
        }

        public async Task<Campanha?> ObterPorIdAsync(int id)
        {
            return await _ctx.Campanha
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<CampanhaAtivaDto>> ListarAtivasAsync()
        {
            return await _ctx.Campanha
                .AsNoTracking()
                .Where(x => x.Status == StatusCampanha.Ativa)
                .OrderByDescending(x => x.DataCriacao)
                .Select(x => new CampanhaAtivaDto
                {
                    Id = x.Id,
                    Titulo = x.Titulo,
                    MetaFinanceira = x.MetaFinanceira,
                    ValorTotalArrecadado =
                        x.ValorTotalArrecadado
                })
                .ToListAsync();
        }

        private static void ValidarDadosCampanha(
            string titulo,
            string descricao,
            DateTime dataInicio,
            DateTime dataFim,
            decimal metaFinanceira)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ApplicationException(
                    "Informe o título da campanha.");

            if (string.IsNullOrWhiteSpace(descricao))
                throw new ApplicationException(
                    "Informe a descrição da campanha.");

            if (dataFim < DateTime.Now)
            {
                throw new ApplicationException(
                    "A data de término da campanha não pode estar no passado.");
            }

            if (dataFim <= dataInicio)
            {
                throw new ApplicationException(
                    "A data final deve ser maior que a data inicial.");
            }

            if (metaFinanceira <= 0)
            {
                throw new ApplicationException(
                    "A meta financeira deve ser maior que zero.");
            }
        }
    }
}