using DataAccessLayer;

namespace MonitorLogic
{
    /// <summary>
    /// Бизнес-логика работы с мониторами.
    /// </summary>
    public class Logic
    {
        private readonly IRepository<DataAccessLayer.MonitorItem> _repository;

        public Logic(IRepository<DataAccessLayer.MonitorItem> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public void CreateMonitor(DataAccessLayer.MonitorItem monitor)
        {
            if (monitor == null) throw new ArgumentNullException(nameof(monitor));
            _repository.Add(monitor);
        }

        public void UpdateMonitor(DataAccessLayer.MonitorItem monitor)
        {
            if (monitor == null) throw new ArgumentNullException(nameof(monitor));
            _repository.Update(monitor);
        }

        public void DeleteMonitor(Guid id)
        {
            _repository.Delete(id);
        }

        public DataAccessLayer.MonitorItem? GetMonitor(Guid id)
        {
            return _repository.ReadById(id);
        }

        public IEnumerable<DataAccessLayer.MonitorItem> GetAllMonitors()
        {
            return _repository.ReadAll();
        }

        /// <summary>
        /// Группировка по производителю.
        /// </summary>
        public IDictionary<string, List<DataAccessLayer.MonitorItem>> GroupMonitorsByManufacturer()
        {
            return _repository
                .ReadAll()
                .GroupBy(m => m.Manufacturer)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        /// <summary>
        /// Мониторы с истекшей гарантией.
        /// </summary>
        public IEnumerable<DataAccessLayer.MonitorItem> GetOutOfWarrantyMonitors()
        {
            return _repository.ReadAll()
                .Where(m => m.PurchaseDate.HasValue &&
                            m.PurchaseDate.Value.AddMonths(m.WarrantyMonths) < DateTime.Now);
        }
    }
}
