using System;
using System.Linq;
using System.Windows.Forms;
using MonitorLogic;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;


namespace MonitorWinForms
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // Настройка Entity Framework
            var options = new DbContextOptionsBuilder<MonitorDbContext>()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MonitorDb;Trusted_Connection=true;MultipleActiveResultSets=true")
                .Options;
            
            var context = new MonitorDbContext(options);
            context.Database.EnsureCreated(); // Создаем БД если не существует
            
            var repo = new EntityRepository<DataAccessLayer.MonitorItem>(context);
            var logic = new Logic(repo);

            // Проверяем, есть ли уже данные в БД, если нет - добавляем тестовые
            if (!logic.GetAllMonitors().Any())
            {
                logic.CreateMonitor(new DataAccessLayer.MonitorItem { Manufacturer = "Dell", Model = "U2419H", SizeInInches = 24, Resolution = "1920x1080", PanelType = "IPS", PurchaseDate = DateTime.Today.AddYears(-2), WarrantyMonths = 24, Note = "Office A" });
                logic.CreateMonitor(new DataAccessLayer.MonitorItem { Manufacturer = "Samsung", Model = "S27R750", SizeInInches = 27, Resolution = "2560x1440", PanelType = "VA", PurchaseDate = DateTime.Today.AddYears(-1), WarrantyMonths = 36, Note = "Design" });
            }

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(logic));
        }
    }
}