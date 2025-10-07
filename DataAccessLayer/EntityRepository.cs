using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    /// <summary>
    /// Репозиторий для работы с данными через Entity Framework.
    /// </summary>
    /// <typeparam name="T">Тип сущности, реализующий IDomainObject</typeparam>
    public class EntityRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly MonitorDbContext _context;
        private readonly DbSet<T> _dbSet;

        public EntityRepository(MonitorDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        public void Add(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                _context.SaveChanges();
            }
        }

        public IEnumerable<T> ReadAll()
        {
            return _dbSet.ToList();
        }

        public T? ReadById(Guid id)
        {
            return _dbSet.Find(id);
        }

        public void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _dbSet.Update(entity);
            _context.SaveChanges();
        }
    }
}
