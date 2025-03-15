namespace ItConsultations.Business.DataAccess.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<T> CreateAsync(T entity);

    Task<T> UpdateAsync(T entity);

    Task DeleteAsync(T entity);
}
