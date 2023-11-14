namespace SampleApi.Common.Core.Repository;

// Interfaces/IRepository.cs
public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAll();
    Task<T?> GetById(Guid id);
    Task Add(T entity);
    Task Update(T entity);
    Task Delete(Guid id);
}