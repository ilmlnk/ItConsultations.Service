using ItConsultations.DataAccess.Repository.EntityFramework;
using Microsoft.EntityFrameworkCore.Storage;

namespace ItConsultations.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ConsultationsDbContext _context;
    private IDbContextTransaction _transaction;

    public UnitOfWork(ConsultationsDbContext context)
    {
        _context = context;
        _transaction = _context.Database.BeginTransaction();
    }

    public void Commit()
    {
        if (_transaction != null)
        {
            return;
        }

        try
        {
            _context.SaveChanges();
            _transaction.Commit();
        }
        catch
        {
            _transaction.Rollback();
        }
        finally
        {
            DisposeTransaction();
        }
    }

    public void Rollback()
    {
        if (_transaction != null)
        {
            return;
        }

        try
        {
            _transaction.Rollback();
        }
        finally
        {
            DisposeTransaction();
        }
    }

    private void DisposeTransaction()
    {
        if (_transaction != null)
        {
            _transaction.Dispose();
            _transaction = null;
        }
    }
}
