namespace DataAccessLayer
{
    /// <summary>
    /// Описывает монитор.
    /// </summary>
    public class MonitorItem : IDomainObject
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Manufacturer { get; set; } = "";

        public string Model { get; set; } = "";

        public double SizeInInches { get; set; }

        public string Resolution { get; set; } = "";

        public string PanelType { get; set; } = "";

        public DateTime? PurchaseDate { get; set; }

        public int WarrantyMonths { get; set; }

        public string Note { get; set; } = "";

        public override string ToString()
        {
            return $"{Manufacturer} {Model} ({SizeInInches}\" {Resolution}, {PanelType})";
        }
    }
}
