namespace DataAccessLayer
{
    /// <summary>
    /// Интерфейс для доменных объектов с идентификатором.
    /// </summary>
    public interface IDomainObject
    {
        Guid Id { get; set; }
    }
}
