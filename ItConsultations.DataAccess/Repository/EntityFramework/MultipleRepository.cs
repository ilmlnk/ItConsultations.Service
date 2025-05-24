using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace ItConsultations.DataAccess.Repository.EntityFramework;

public class MultipleRepository<T> : BaseRepository<T>, IMultipleRepository<T>
    where T : class
{
    public MultipleRepository(ConsultationsDbContext context) : base(context) { }

    public virtual async Task<IEnumerable<T>> CreateAsync(IEnumerable<T> entities)
    {
        var entityList = entities.ToList();
        await _entities.AddRangeAsync(entityList);
        SaveChanges();
        return await Task.FromResult(entityList);
    }

    public virtual async Task DeleteAsync(IEnumerable<T> entities)
    {
        var entityList = entities.ToList();
        _entities.RemoveRange(entityList);
        SaveChanges();
        await Task.CompletedTask;
    }

    public void Attach(IEntity<long> entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _context.Attach(entity);
        }
    }

    public void Dettach(IEntity<long> entity)
    {
        _context.Entry(entity).State = EntityState.Detached;
    }

    public TEntity FindTracked<TEntity>(params object[] keyValues)
    {
        var entityType = _context.Model.FindEntityType(typeof(TEntity));
        var key = entityType.FindPrimaryKey();
        var stateManager = _context.GetDependencies().StateManager;
        var entry = stateManager.TryGetEntry(key);
        return (TEntity)(entry?.Entity);
    }

    public async Task<IEnumerable<T>> UpdateAsync(IEnumerable<T> entities)
    {
        var entityList = entities.ToList();
        _entities.UpdateRange(entityList);
        SaveChanges();
        return await Task.FromResult(entityList);
    }
}
