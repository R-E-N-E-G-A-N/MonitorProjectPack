using System;

namespace MonitorModel
{
    /// <summary>Монитор (запись).</summary>
    public class MonitorItem
    {
        /// <summary>Идентификатор.</summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>Производитель.</summary>
        public string Manufacturer { get; set; } = string.Empty;

        /// <summary>Модель.</summary>
        public string Model { get; set; } = string.Empty;

        /// <summary>Диагональ в дюймах.</summary>
        public double SizeInInches { get; set; }

        /// <summary>Разрешение.</summary>
        public string Resolution { get; set; } = string.Empty;

        /// <summary>Тип панели.</summary>
        public string PanelType { get; set; } = string.Empty;

        /// <summary>Дата покупки.</summary>
        public DateTime? PurchaseDate { get; set; }

        /// <summary>Гарантия (месяцев).</summary>
        public int WarrantyMonths { get; set; }

        /// <summary>Примечание.</summary>
        public string Note { get; set; } = string.Empty;

        public override string ToString()
            => $"{Manufacturer} {Model} {SizeInInches}\" {Resolution} ({PanelType}) Id:{Id}";
    }
}