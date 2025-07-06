using ItConsultations.Business.Entities;

namespace ItConsultations.Business.DataAccess.Interfaces;

public interface IMultipleRepository<T> : IBaseRepository<T> where T : class
{
    Task DeleteAsync(IEnumerable<T> entities);

    Task<IEnumerable<T>> UpdateAsync(IEnumerable<T> entities);

    TEntity FindTracked<TEntity>(params object[] keyValues);

    void Attach(IEntity<long> entity);

    void Dettach(IEntity<long> entity);
} 