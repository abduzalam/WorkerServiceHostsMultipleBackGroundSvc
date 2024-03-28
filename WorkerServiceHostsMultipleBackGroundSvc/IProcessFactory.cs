namespace WorkerServiceHostsMultipleBackGroundSvc
{
    public interface IProcessFactory
    {
        IProcess CreateProcess(ProcessType type);
    }
}
