using System;
using System.Collections.Generic;
using System.Linq;

namespace MonitorModel
{
    /// <summary>In-memory реализация репозитория.</summary>
    public class InMemoryMonitorRepository : IMonitorRepository
    {
        private readonly List<MonitorItem> _store = new();

        public MonitorItem Create(MonitorItem monitor)
        {
            if (monitor.Id == Guid.Empty) monitor.Id = Guid.NewGuid();
            _store.Add(Clone(monitor));
            return monitor;
        }

        public MonitorItem? Get(Guid id) => _store.FirstOrDefault(m => m.Id == id);

        public IEnumerable<MonitorItem> GetAll() => _store.Select(Clone).ToList();

        public void Update(MonitorItem monitor)
        {
            var existing = _store.FirstOrDefault(m => m.Id == monitor.Id);
            if (existing == null) throw new InvalidOperationException("Monitor not found");
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
            var existing = _store.FirstOrDefault(m => m.Id == id);
            if (existing != null) _store.Remove(existing);
        }

        private static MonitorItem Clone(MonitorItem src) => new MonitorItem
        {
            Id = src.Id,
            Manufacturer = src.Manufacturer,
            Model = src.Model,
            SizeInInches = src.SizeInInches,
            Resolution = src.Resolution,
            PanelType = src.PanelType,
            PurchaseDate = src.PurchaseDate,
            WarrantyMonths = src.WarrantyMonths,
            Note = src.Note
        };
    }
}