namespace MonitorLogic
{
    /// <summary>
    /// Интерфейс репозитория для хранения мониторов.
    /// </summary>
    public interface IMonitorRepository
    {
        void Add(MonitorItem monitor);
        void Update(MonitorItem monitor);
        void Delete(Guid id);
        MonitorItem? GetById(Guid id);
        IEnumerable<MonitorItem> GetAll();
    }
}
