namespace WorkerServiceHostsMultipleBackGroundSvc
{
    public class ThirdWorker : IProcess
    {
        private readonly ILogger<ThirdWorker> _logger;
        public ThirdWorker(ILogger<ThirdWorker> logger)
        {
            _logger = logger;
        }

        public async Task ProcessRequest(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ThirdWorker running at: {time}", DateTimeOffset.Now);
            await Task.CompletedTask;
        }
    }
}
