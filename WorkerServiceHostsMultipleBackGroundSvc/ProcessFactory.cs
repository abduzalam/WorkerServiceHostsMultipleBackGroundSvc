namespace WorkerServiceHostsMultipleBackGroundSvc
{
    public class ProcessFactory : IProcessFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ProcessFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IProcess CreateProcess(ProcessType type)
        {
            return type switch
            {
                ProcessType.First => _serviceProvider.GetRequiredService<FirstWorker>(),
                ProcessType.Second => _serviceProvider.GetRequiredService<SecondWorker>(),
                ProcessType.Third => _serviceProvider.GetRequiredService<ThirdWorker>(),
                _ => throw new ArgumentException("Invalid process type", nameof(type)),
            };
        }
    }
}
