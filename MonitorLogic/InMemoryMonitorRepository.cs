namespace MonitorLogic
{
    /// <summary>
    /// Хранение мониторов в памяти.
    /// </summary>
    public class InMemoryMonitorRepository : IMonitorRepository
    {
        private readonly List<MonitorItem> _monitors = new();

        public void Add(MonitorItem monitor)
        {
            _monitors.Add(monitor);
        }

        public void Update(MonitorItem monitor)
        {
            var existing = GetById(monitor.Id);
            if (existing == null) return;

            existing.Manufacturer = monitor.Manufacturer;
            existing.Model = monitor.Model;
            existing.SizeInInches = monitor.SizeInInches;
            existing.Resolution = monitor.Resolution;
            existing.PanelType = monitor.PanelType;
            existing.PurchaseDate = monitor.PurchaseDate;
            existing.WarrantyMonths = monitor.WarrantyMonths;
            existing.Note = monitor.Note;
        }

        public void Delete(Guid id)
        {
            var existing = GetById(id);
            if (existing != null)
                _monitors.Remove(existing);
        }

        public MonitorItem? GetById(Guid id)
        {
            return _monitors.FirstOrDefault(m => m.Id == id);
        }

        public IEnumerable<MonitorItem> GetAll()
        {
            return _monitors;
        }
    }
}
