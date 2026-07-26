using Domain.Events;

namespace ConexaoSolidaria.Worker.Services
{
    public interface IDoacaoProcessor
    {
        Task ProcessarAsync(
            DoacaoRecebidaEvent evento,
            CancellationToken cancellationToken);
    }
}