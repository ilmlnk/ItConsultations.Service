using ItConsultations.Business.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ItConsultations.DataAccess.Repository.EntityFramework;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly ConsultationsDbContext _context;
    protected readonly DbSet<T> _entities;

    public BaseRepository(ConsultationsDbContext context)
    {
        _context = context;
        _entities = context.Set<T>();
    }
    public virtual async Task<T> CreateAsync(T entity)
    {
        var e = await _entities.AddAsync(entity);
        await SaveChangesAsync();
        return e.Entity;
    }

    public virtual async Task DeleteAsync(T entity)
    {
        _entities.Remove(entity);
        await SaveChangesAsync();
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        _entities.Update(entity);
        await SaveChangesAsync();
        return entity;
    }

    protected virtual async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
