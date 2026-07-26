namespace ConexaoSolidaria.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Worker de doações iniciado.");

            await Task.Delay(
                Timeout.Infinite,
                stoppingToken);
        }
    }
}