using Dapper;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    /// <summary>
    /// Репозиторий для работы с данными через Dapper.
    /// </summary>
    /// <typeparam name="T">Тип сущности, реализующий IDomainObject</typeparam>
    public class DapperRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly string _connectionString;

        public DapperRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public void Add(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            
            using var connection = new SqlConnection(_connectionString);
            
            // Для MonitorItem создаем специфичный SQL
            if (entity is MonitorItem monitor)
            {
                var sql = @"
                    INSERT INTO Monitors (Id, Manufacturer, Model, SizeInInches, Resolution, PanelType, PurchaseDate, WarrantyMonths, Note)
                    VALUES (@Id, @Manufacturer, @Model, @SizeInInches, @Resolution, @PanelType, @PurchaseDate, @WarrantyMonths, @Note)";
                
                connection.Execute(sql, monitor);
            }
        }

        public void Delete(Guid id)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var sql = "DELETE FROM Monitors WHERE Id = @Id";
            connection.Execute(sql, new { Id = id });
        }

        public IEnumerable<T> ReadAll()
        {
            using var connection = new SqlConnection(_connectionString);
            
            var sql = "SELECT * FROM Monitors";
            return connection.Query<T>(sql);
        }

        public T? ReadById(Guid id)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var sql = "SELECT * FROM Monitors WHERE Id = @Id";
            return connection.QueryFirstOrDefault<T>(sql, new { Id = id });
        }

        public void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            
            using var connection = new SqlConnection(_connectionString);
            
            // Для MonitorItem создаем специфичный SQL
            if (entity is MonitorItem monitor)
            {
                var sql = @"
                    UPDATE Monitors 
                    SET Manufacturer = @Manufacturer, 
                        Model = @Model, 
                        SizeInInches = @SizeInInches, 
                        Resolution = @Resolution, 
                        PanelType = @PanelType, 
                        PurchaseDate = @PurchaseDate, 
                        WarrantyMonths = @WarrantyMonths, 
                        Note = @Note
                    WHERE Id = @Id";
                
                connection.Execute(sql, monitor);
            }
        }
    }
}
