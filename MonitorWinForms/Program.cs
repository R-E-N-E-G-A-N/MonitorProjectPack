using System;
using System.Windows.Forms;
using MonitorLogic;


namespace MonitorWinForms
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var repo = new InMemoryMonitorRepository();
            var logic = new Logic(repo);

            // seed
            logic.CreateMonitor(new MonitorItem { Manufacturer = "Dell", Model = "U2419H", SizeInInches = 24, Resolution = "1920x1080", PanelType = "IPS", PurchaseDate = DateTime.Today.AddYears(-2), WarrantyMonths = 24, Note = "Office A" });
            logic.CreateMonitor(new MonitorItem { Manufacturer = "Samsung", Model = "S27R750", SizeInInches = 27, Resolution = "2560x1440", PanelType = "VA", PurchaseDate = DateTime.Today.AddYears(-1), WarrantyMonths = 36, Note = "Design" });

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(logic));
        }
    }
}