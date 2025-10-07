using System;
using System.Linq;
using MonitorLogic;
using DataAccessLayer;

namespace MonitorConsole
{
    /// <summary>
    /// Пример использования Dapper репозитория.
    /// </summary>
    class ProgramWithDapper
    {
        static void MainDapper()
        {
            // Настройка Dapper
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=MonitorDb;Trusted_Connection=true;MultipleActiveResultSets=true";
            var repo = RepositoryFactory.CreateDapperRepository(connectionString);
            var logic = new Logic(repo);
            SeedData(logic);

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Система учёта мониторов (Dapper):");
                Console.WriteLine("1) Показать все мониторы");
                Console.WriteLine("2) Добавить монитор");
                Console.WriteLine("3) Редактировать монитор (выбор по индексу)");
                Console.WriteLine("4) Удалить монитор (выбор по индексу)");
                Console.WriteLine("5) Сгруппировать по производителю");
                Console.WriteLine("6) Показать просроченные по гарантии");
                Console.WriteLine("0) Выход");
                Console.Write("Выберите пункт: ");
                var choice = Console.ReadLine()?.Trim();

                try
                {
                    switch (choice)
                    {
                        case "1": ListAll(logic); break;
                        case "2": CreateInteractive(logic); break;
                        case "3": EditInteractive(logic); break;
                        case "4": DeleteInteractive(logic); break;
                        case "5": GroupByManufacturerInteractive(logic); break;
                        case "6": ShowOutOfWarrantyInteractive(logic); break;
                        case "0": return;
                        default: Console.WriteLine("Неверный выбор."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
        }

        static void SeedData(Logic logic)
        {
            logic.CreateMonitor(new DataAccessLayer.MonitorItem
            {
                Manufacturer = "Dell",
                Model = "U2419H",
                SizeInInches = 24.0,
                Resolution = "1920x1080",
                PanelType = "IPS",
                PurchaseDate = DateTime.Today.AddYears(-2),
                WarrantyMonths = 24,
                Note = "Офис A"
            });

            logic.CreateMonitor(new DataAccessLayer.MonitorItem
            {
                Manufacturer = "Samsung",
                Model = "S27R750",
                SizeInInches = 27.0,
                Resolution = "2560x1440",
                PanelType = "VA",
                PurchaseDate = DateTime.Today.AddYears(-1),
                WarrantyMonths = 36,
                Note = "Дизайн"
            });
        }

        static void ListAll(Logic logic)
        {
            var list = logic.GetAllMonitors().ToList();
            if (!list.Any()) { Console.WriteLine("Список пуст."); return; }
            for (int i = 0; i < list.Count; i++) Console.WriteLine($"[{i}] {list[i]}");
        }

        static void CreateInteractive(Logic logic)
        {
            var m = ReadMonitorFromConsole(new DataAccessLayer.MonitorItem());
            logic.CreateMonitor(m);
            Console.WriteLine("Создан: " + m);
        }

        static DataAccessLayer.MonitorItem ReadMonitorFromConsole(DataAccessLayer.MonitorItem baseModel)
        {
            Console.Write($"Производитель ({baseModel.Manufacturer}): ");
            var manufacturer = ReadRequiredString(baseModel.Manufacturer);
            Console.Write($"Модель ({baseModel.Model}): ");
            var model = ReadRequiredString(baseModel.Model);
            Console.Write($"Диагональ в дюймах ({baseModel.SizeInInches}): ");
            var size = ReadDoubleOrDefault(baseModel.SizeInInches);
            Console.Write($"Разрешение ({baseModel.Resolution}): ");
            var resolution = Console.ReadLine()?.Trim() ?? baseModel.Resolution;
            Console.Write($"Тип панели ({baseModel.PanelType}): ");
            var panel = Console.ReadLine()?.Trim() ?? baseModel.PanelType;
            Console.Write($"Дата покупки (гггг-MM-dd) ({baseModel.PurchaseDate?.ToString("yyyy-MM-dd") ?? "нет"}): ");
            var purchase = ReadDateOrNull(baseModel.PurchaseDate);
            Console.Write($"Гарантия (месяцев) ({baseModel.WarrantyMonths}): ");
            var warranty = ReadIntOrDefault(baseModel.WarrantyMonths);
            Console.Write($"Примечание ({baseModel.Note}): ");
            var note = Console.ReadLine()?.Trim() ?? baseModel.Note;

            return new DataAccessLayer.MonitorItem
            {
                Id = baseModel.Id,
                Manufacturer = manufacturer,
                Model = model,
                SizeInInches = size,
                Resolution = resolution,
                PanelType = panel,
                PurchaseDate = purchase,
                WarrantyMonths = warranty,
                Note = note
            };
        }

        static void EditInteractive(Logic logic)
        {
            var list = logic.GetAllMonitors().ToList();
            if (!list.Any()) { Console.WriteLine("Нет записей."); return; }
            ListAll(logic);
            Console.Write("Введите индекс для редактирования: ");
            if (!int.TryParse(Console.ReadLine(), out var idx) || idx < 0 || idx >= list.Count) { Console.WriteLine("Неверный индекс."); return; }
            var selected = list[idx];
            var edited = ReadMonitorFromConsole(selected);
            logic.UpdateMonitor(edited);
            Console.WriteLine("Обновлено.");
        }

        static void DeleteInteractive(Logic logic)
        {
            var list = logic.GetAllMonitors().ToList();
            if (!list.Any()) { Console.WriteLine("Нет записей."); return; }
            ListAll(logic);
            Console.Write("Введите индекс для удаления: ");
            if (!int.TryParse(Console.ReadLine(), out var idx) || idx < 0 || idx >= list.Count) { Console.WriteLine("Неверный индекс."); return; }
            var selected = list[idx];
            Console.Write($"Удалить {selected.Manufacturer} {selected.Model}? (y/n): ");
            var ans = Console.ReadLine()?.Trim().ToLower();
            if (ans == "y" || ans == "д" || ans == "yes")
            {
                logic.DeleteMonitor(selected.Id);
                Console.WriteLine("Удалено.");
            }
            else Console.WriteLine("Отмена.");
        }

        static void GroupByManufacturerInteractive(Logic logic)
        {
            var groups = logic.GroupMonitorsByManufacturer();
            foreach (var kv in groups)
            {
                Console.WriteLine($"{kv.Key} ({kv.Value.Count})");
                foreach (var m in kv.Value) Console.WriteLine($"  - {m}");
            }
        }

        static void ShowOutOfWarrantyInteractive(Logic logic)
        {
            var expired = logic.GetOutOfWarrantyMonitors().ToList();
            if (!expired.Any()) { Console.WriteLine("Нет мониторов с истёкшей гарантией."); return; }
            foreach (var m in expired) Console.WriteLine(m);
        }

        #region Helpers
        static string ReadRequiredString(string defaultValue)
        {
            var input = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(input))
            {
                if (!string.IsNullOrWhiteSpace(defaultValue)) return defaultValue;
                Console.Write("Это поле обязательно. Введите значение: ");
                return ReadRequiredString(defaultValue);
            }
            return input;
        }

        static double ReadDoubleOrDefault(double defaultValue)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line)) return defaultValue;
            if (double.TryParse(line.Trim(), out var v) && v > 0) return v;
            Console.Write("Неверный ввод. Введите число > 0: ");
            return ReadDoubleOrDefault(defaultValue);
        }

        static int ReadIntOrDefault(int defaultValue)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line)) return defaultValue;
            if (int.TryParse(line.Trim(), out var v) && v >= 0) return v;
            Console.Write("Неверный ввод. Введите неотрицательное целое: ");
            return ReadIntOrDefault(defaultValue);
        }

        static DateTime? ReadDateOrNull(DateTime? defaultValue)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line)) return defaultValue;
            if (DateTime.TryParse(line.Trim(), out var d)) return d.Date;
            Console.Write("Неверная дата. Введите гггг-MM-dd или пустую строку: ");
            return ReadDateOrNull(defaultValue);
        }
        #endregion
    }
}
