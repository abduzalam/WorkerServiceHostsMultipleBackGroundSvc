namespace WorkerServiceHostsMultipleBackGroundSvc
{
    public class SecondWorker : IProcess
    {
        private readonly ILogger<SecondWorker> _logger;
        public SecondWorker(ILogger<SecondWorker> logger)
        {
            _logger = logger;
        }

        public async Task ProcessRequest(CancellationToken stoppingToken)
        {
            _logger.LogInformation("SecondWorker running at: {time}", DateTimeOffset.Now);
            await Task.CompletedTask;
        }
    }
}
