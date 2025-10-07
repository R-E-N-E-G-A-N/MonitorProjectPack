namespace DataAccessLayer
{
    /// <summary>
    /// Интерфейс репозитория для основных CRUD операций.
    /// </summary>
    /// <typeparam name="T">Тип сущности, реализующий IDomainObject</typeparam>
    public interface IRepository<T> where T : class, IDomainObject
    {
        /// <summary>
        /// Добавляет новую сущность.
        /// </summary>
        /// <param name="entity">Сущность для добавления</param>
        void Add(T entity);

        /// <summary>
        /// Удаляет сущность по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности</param>
        void Delete(Guid id);

        /// <summary>
        /// Получает все сущности.
        /// </summary>
        /// <returns>Коллекция всех сущностей</returns>
        IEnumerable<T> ReadAll();

        /// <summary>
        /// Получает сущность по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности</param>
        /// <returns>Сущность или null, если не найдена</returns>
        T? ReadById(Guid id);

        /// <summary>
        /// Обновляет существующую сущность.
        /// </summary>
        /// <param name="entity">Сущность для обновления</param>
        void Update(T entity);
    }
}
