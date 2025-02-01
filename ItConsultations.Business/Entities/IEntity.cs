namespace ItConsultations.Business.Entities;

public interface IEntity<TID>
{
    TID Id { get; set; }
    DateTime CreatedAt { get; set; }
    Coach Coach { get; set; }
}
