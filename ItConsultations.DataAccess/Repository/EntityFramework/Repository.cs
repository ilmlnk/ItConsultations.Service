using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Entities;
using ItConsultations.Business.Entities.Consultation;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ItConsultations.DataAccess.Repository.EntityFramework;

public class Repository<T, TID> : MultipleRepository<T>, IRepository<T, TID>
    where T : class, IEntity<TID>
{
    public Repository(ConsultationsDbContext context) : base(context) { }

    public async Task<bool> Exists(TID id)
    {
        return await _entities.FindAsync(id) != null;
    }

    public void Detach(Consultation consultation)
    {
        _context.Entry(consultation).State = EntityState.Detached;
    }

    public async Task<T> GetAsync(TID id)
    {
        return await _entities.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _entities.ToListAsync();
    }

    public IQueryable<T> Get(Expression<Func<T, bool>> expression)
    {
        return _entities.Where(expression);
    }

    public IQueryable<T> Include<TProperty>(Expression<Func<T, TProperty>> entities)
    {
        return _entities.Include(entities);
    }

    public async Task<IEnumerable<T>> GetAsync(IEnumerable<TID> ids)
    {
        return await _entities.Where(e => ids.Contains(e.Id)).ToListAsync();
    }

    public async Task<T> GetWithAsync<TProperty>(TID id, Expression<Func<T, TProperty>> subEntity)
    {
        return await _entities.Include(subEntity).FirstOrDefaultAsync(e => e.Id.Equals(id));
    }
} 