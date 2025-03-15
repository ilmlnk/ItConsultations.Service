using ItConsultations.Business.Entities;
using System.Linq.Expressions;

namespace ItConsultations.Business.DataAccess.Interfaces;

public interface IReadOnlyRepository<T, in TID> where T : class, IEntity<TID>
{
    Task<T> GetAsync(TID id);
    IQueryable<T> Get(Expression<Func<T, bool>> expression);
    IQueryable<T> Include<TProperty>(Expression<Func<T, TProperty>> entities);

    Task<IEnumerable<T>> GetAsync(IEnumerable<TID> ids);

    Task<T> GetWithAsync<TProperty>(TID id, Expression<Func<T, TProperty>> subEntity);
}
