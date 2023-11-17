namespace SampleApi.Common.Infrastructure.Data.Repository;

// Interfaces/IRepository.cs
public interface IRepository<T>
{
    IEnumerable<T> GetAll();
    T? GetById(Guid id);
    void Add(T entity);
    void Update(T entity);
    void Delete(Guid id);
    void Save();
}