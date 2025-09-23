namespace MonitorLogic
{
    /// <summary>
    /// Бизнес-логика работы с мониторами.
    /// </summary>
    public class Logic
    {
        private readonly IMonitorRepository _repository;

        public Logic(IMonitorRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public void CreateMonitor(MonitorItem monitor)
        {
            if (monitor == null) throw new ArgumentNullException(nameof(monitor));
            _repository.Add(monitor);
        }

        public void UpdateMonitor(MonitorItem monitor)
        {
            if (monitor == null) throw new ArgumentNullException(nameof(monitor));
            _repository.Update(monitor);
        }

        public void DeleteMonitor(Guid id)
        {
            _repository.Delete(id);
        }

        public MonitorItem? GetMonitor(Guid id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<MonitorItem> GetAllMonitors()
        {
            return _repository.GetAll();
        }

        /// <summary>
        /// Группировка по производителю.
        /// </summary>
        public IDictionary<string, List<MonitorItem>> GroupMonitorsByManufacturer()
        {
            return _repository
                .GetAll()
                .GroupBy(m => m.Manufacturer)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        /// <summary>
        /// Мониторы с истекшей гарантией.
        /// </summary>
        public IEnumerable<MonitorItem> GetOutOfWarrantyMonitors()
        {
            return _repository.GetAll()
                .Where(m => m.PurchaseDate.HasValue &&
                            m.PurchaseDate.Value.AddMonths(m.WarrantyMonths) < DateTime.Now);
        }
    }
}
