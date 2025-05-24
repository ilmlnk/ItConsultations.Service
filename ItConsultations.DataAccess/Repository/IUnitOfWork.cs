namespace ItConsultations.DataAccess.Repository;

public interface IUnitOfWork
{
    void Commit();

    void Rollback();
}
