namespace WorkerServiceHostsMultipleBackGroundSvc
{
    public class SecondWorkerService : BackgroundService
    {
        private readonly ILogger<SecondWorkerService> _logger;
        private readonly IProcess _process;
        public SecondWorkerService(ILogger<SecondWorkerService> logger, IProcess process)
        {
            _logger = logger;
            _process = process;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    if (_logger.IsEnabled(LogLevel.Information))
                    {
                        _logger.LogInformation("SecondWorkerService running at: {time}", DateTimeOffset.Now);
                        await _process.ProcessRequest(stoppingToken);
                    }
                    await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);
                }
            }
            catch (TaskCanceledException)
            {
                // When the stopping token is canceled, for example, a call made from services.msc,
                // we shouldn't exit with a non-zero exit code. In other words, this is expected...
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);

                // Terminates this process and returns an exit code to the operating system.
                // This is required to avoid the 'BackgroundServiceExceptionBehavior', which
                // performs one of two scenarios:
                // 1. When set to "Ignore": will do nothing at all, errors cause zombie services.
                // 2. When set to "StopHost": will cleanly stop the host, and log errors.
                //
                // In order for the Windows Service Management system to leverage configured
                // recovery options, we need to terminate the process with a non-zero exit code.
                Environment.Exit(1);
            }
        }
    }
   
}
