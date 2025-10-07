using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    /// <summary>
    /// Фабрика для создания репозиториев.
    /// </summary>
    public static class RepositoryFactory
    {
        /// <summary>
        /// Создает Entity Framework репозиторий.
        /// </summary>
        /// <param name="connectionString">Строка подключения к БД</param>
        /// <returns>Entity Framework репозиторий</returns>
        public static IRepository<MonitorItem> CreateEntityFrameworkRepository(string connectionString)
        {
            var options = new DbContextOptionsBuilder<MonitorDbContext>()
                .UseSqlServer(connectionString)
                .Options;
            
            var context = new MonitorDbContext(options);
            context.Database.EnsureCreated();
            
            return new EntityRepository<MonitorItem>(context);
        }

        /// <summary>
        /// Создает Dapper репозиторий.
        /// </summary>
        /// <param name="connectionString">Строка подключения к БД</param>
        /// <returns>Dapper репозиторий</returns>
        public static IRepository<MonitorItem> CreateDapperRepository(string connectionString)
        {
            return new DapperRepository<MonitorItem>(connectionString);
        }
    }
}
