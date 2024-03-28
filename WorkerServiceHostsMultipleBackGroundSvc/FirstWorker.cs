namespace WorkerServiceHostsMultipleBackGroundSvc
{
    public class FirstWorker : IProcess
    {
        private readonly ILogger<FirstWorker> _logger;
        public FirstWorker(ILogger<FirstWorker> logger)
        {
            _logger = logger;
        }
     
        public async Task ProcessRequest(CancellationToken stoppingToken)
        {
            _logger.LogInformation("FirstWorker running at: {time}", DateTimeOffset.Now);
             await Task.CompletedTask;
        }
    }
}
