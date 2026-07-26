using ConexaoSolidaria.Worker.Data;
using ConexaoSolidaria.Worker.Entities;
using Domain.Events;
using Microsoft.EntityFrameworkCore;

namespace ConexaoSolidaria.Worker.Services
{
    public class DoacaoProcessor : IDoacaoProcessor
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DoacaoProcessor> _logger;

        public DoacaoProcessor(
            AppDbContext context,
            ILogger<DoacaoProcessor> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task ProcessarAsync(
            DoacaoRecebidaEvent evento,
            CancellationToken cancellationToken)
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync(
                    cancellationToken);

            try
            {
                var doacao = await _context.Doacoes
                    .FirstOrDefaultAsync(
                        x => x.Id == evento.DoacaoId,
                        cancellationToken);

                if (doacao == null)
                {
                    _logger.LogWarning(
                        "Doação {DoacaoId} não encontrada.",
                        evento.DoacaoId);

                    await transaction.RollbackAsync(cancellationToken);
                    return;
                }

                if (doacao.Status == StatusDoacao.Processada)
                {
                    _logger.LogInformation(
                        "Doação {DoacaoId} já foi processada.",
                        doacao.Id);

                    await transaction.RollbackAsync(cancellationToken);
                    return;
                }

                var campanha = await _context.Campanhas
                    .FirstOrDefaultAsync(
                        x => x.Id == doacao.CampanhaId,
                        cancellationToken);

                if (campanha == null)
                {
                    doacao.Status = StatusDoacao.Rejeitada;
                    doacao.DataProcessamento = DateTime.UtcNow;

                    await _context.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);

                    _logger.LogWarning(
                        "Campanha {CampanhaId} não encontrada. Doação {DoacaoId} rejeitada.",
                        doacao.CampanhaId,
                        doacao.Id);

                    return;
                }

                campanha.ValorArrecadado += doacao.ValorDoacao;

                doacao.Status = StatusDoacao.Processada;
                doacao.DataProcessamento = DateTime.UtcNow;

                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                _logger.LogInformation(
                    "Doação {DoacaoId} processada. Novo valor arrecadado da campanha {CampanhaId}: {ValorArrecadado}",
                    doacao.Id,
                    campanha.Id,
                    campanha.ValorArrecadado);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);

                _logger.LogError(
                    ex,
                    "Erro ao processar a doação {DoacaoId}.",
                    evento.DoacaoId);

                throw;
            }
        }
    }
}