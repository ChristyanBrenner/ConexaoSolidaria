using ConexaoSolidaria.Worker.Services;
using Domain.Events;
using MassTransit;

namespace ConexaoSolidaria.Worker.Consumers
{
    public class DoacaoCriadaConsumer : IConsumer<DoacaoRecebidaEvent>
    {
        private readonly IDoacaoProcessor _doacaoProcessor;
        private readonly ILogger<DoacaoCriadaConsumer> _logger;

        public DoacaoCriadaConsumer(
            IDoacaoProcessor doacaoProcessor,
            ILogger<DoacaoCriadaConsumer> logger)
        {
            _doacaoProcessor = doacaoProcessor;
            _logger = logger;
        }

        public async Task Consume(
            ConsumeContext<DoacaoRecebidaEvent> context)
        {
            var evento = context.Message;

            _logger.LogInformation(
                "Evento recebido. DoacaoId: {DoacaoId}, CampanhaId: {CampanhaId}, Valor: {ValorDoacao}",
                evento.DoacaoId,
                evento.CampanhaId,
                evento.ValorDoacao);

            await _doacaoProcessor.ProcessarAsync(
                evento,
                context.CancellationToken);
        }
    }
}