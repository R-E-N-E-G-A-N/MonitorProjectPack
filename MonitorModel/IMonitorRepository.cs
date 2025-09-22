using System;
using System.Collections.Generic;

namespace MonitorModel
{
    /// <summary>Репозиторий для MonitorItem.</summary>
    public interface IMonitorRepository
    {
        /// <summary>Создать.</summary>
        MonitorItem Create(MonitorItem monitor);

        /// <summary>Получить по id.</summary>
        MonitorItem? Get(Guid id);

        /// <summary>Все записи.</summary>
        IEnumerable<MonitorItem> GetAll();

        /// <summary>Обновить.</summary>
        void Update(MonitorItem monitor);

        /// <summary>Удалить.</summary>
        void Delete(Guid id);
    }
}