namespace ItConsultations.Business.Entities;

public abstract class Entity<TID> : IEntity<TID>
{
    protected Entity()
    {
        CreatedAt = DateTime.UtcNow;
    }

    public virtual TID Id { get; set; }
    public virtual DateTime CreatedAt { get; set; }
}
