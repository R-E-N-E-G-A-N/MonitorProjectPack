using System;
using System.Collections.Generic;
using System.Linq;
using MonitorModel;

namespace MonitorLogic
{
    /// <summary>Бизнес-логика для мониторов.</summary>
    public class Logic
    {
        private readonly IMonitorRepository _repo;

        /// <summary>Конструктор.</summary>
        public Logic(IMonitorRepository repository)
        {
            _repo = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>Создать монитор.</summary>
        public MonitorItem CreateMonitor(MonitorItem monitor)
        {
            ValidateForCreate(monitor);
            return _repo.Create(monitor);
        }

        /// <summary>Получить по id.</summary>
        public MonitorItem? GetMonitor(Guid id) => _repo.Get(id);

        /// <summary>Все мониторы.</summary>
        public IEnumerable<MonitorItem> GetAllMonitors() => _repo.GetAll();

        /// <summary>Обновить монитор.</summary>
        public void UpdateMonitor(MonitorItem monitor)
        {
            if (monitor == null) throw new ArgumentNullException(nameof(monitor));
            if (monitor.Id == Guid.Empty) throw new ArgumentException("Id обязателен для обновления", nameof(monitor));
            ValidateBasic(monitor);
            _repo.Update(monitor);
        }

        /// <summary>Удалить по id.</summary>
        public void DeleteMonitor(Guid id) => _repo.Delete(id);

        /// <summary>Группировать по производителю.</summary>
        public IDictionary<string, List<MonitorItem>> GroupMonitorsByManufacturer()
        {
            return _repo.GetAll()
                        .GroupBy(m => string.IsNullOrWhiteSpace(m.Manufacturer) ? "Unknown" : m.Manufacturer)
                        .ToDictionary(g => g.Key, g => g.ToList());
        }

        /// <summary>Мониторы с истёкшей гарантией.</summary>
        public IEnumerable<MonitorItem> GetOutOfWarrantyMonitors(DateTime? referenceDate = null)
        {
            var date = referenceDate?.Date ?? DateTime.Today;
            return _repo.GetAll()
                        .Where(m => m.PurchaseDate.HasValue && (m.PurchaseDate.Value.AddMonths(m.WarrantyMonths).Date < date))
                        .ToList();
        }

        #region Validation
        private static void ValidateForCreate(MonitorItem monitor)
        {
            if (monitor == null) throw new ArgumentNullException(nameof(monitor));
            ValidateBasic(monitor);
        }

        private static void ValidateBasic(MonitorItem monitor)
        {
            if (string.IsNullOrWhiteSpace(monitor.Manufacturer))
                throw new ArgumentException("Manufacturer обязательен", nameof(monitor.Manufacturer));
            if (string.IsNullOrWhiteSpace(monitor.Model))
                throw new ArgumentException("Model обязательна", nameof(monitor.Model));
            if (monitor.SizeInInches <= 0)
                throw new ArgumentException("SizeInInches должен быть > 0", nameof(monitor.SizeInInches));
            if (monitor.WarrantyMonths < 0)
                throw new ArgumentException("WarrantyMonths >= 0", nameof(monitor.WarrantyMonths));

            monitor.Resolution = monitor.Resolution?.Trim() ?? string.Empty;
            monitor.PanelType = monitor.PanelType?.Trim() ?? string.Empty;
            monitor.Note = monitor.Note?.Trim() ?? string.Empty;
        }
        #endregion
    }
}