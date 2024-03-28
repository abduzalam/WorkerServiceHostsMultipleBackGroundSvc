namespace WorkerServiceHostsMultipleBackGroundSvc
{
    public interface IProcess
    {
        Task ProcessRequest(CancellationToken stoppingToken);
    }
}
