using ItConsultations.Business.Entities.Consultation;

namespace ItConsultations.Business.Entities;

public interface IEntity<TID>
{
    TID Id { get; set; }

    DateTime CreatedAt { get; set; }
}
