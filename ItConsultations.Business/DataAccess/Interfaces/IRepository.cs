using ItConsultations.Business.Entities;
using ItConsultations.Business.Entities.Consultation;

namespace ItConsultations.Business.DataAccess.Interfaces;

public interface IRepository<T, TID> : IReadOnlyRepository<T, TID>, IMultipleRepository<T>
    where T : class, IEntity<TID>
{
    Task<bool> Exists(TID id);

    void Detach(Consultation consultation);
}
