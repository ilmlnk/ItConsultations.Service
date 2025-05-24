using ItConsultations.Business.Entities.Consultation;
using Microsoft.EntityFrameworkCore;

namespace ItConsultations.DataAccess.Repository.EntityFramework;

public class ConsultationsDbContext : DbContext
{
    public ConsultationsDbContext(DbContextOptions<ConsultationsDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Id).IsRequired();
            entity.Property(s => s.FirstName).IsRequired().HasMaxLength(32);
            entity.Property(s => s.LastName).HasMaxLength(32);
            entity.Property(s => s.Email).IsRequired();
            entity.Property(s => s.Username).IsRequired();
            entity.Property(s => s.Password).IsRequired();

            entity.HasOne(s => s.Consultation)
                  .WithMany(c => c.Students);
        });

        modelBuilder.Entity<Consultation>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).IsRequired();
            entity.Property(c => c.Title).IsRequired();
            entity.Property(c => c.Price).IsRequired();
            entity.Property(c => c.Duration).IsRequired();

            entity.HasOne(c => c.Coach)
                  .WithMany();

            entity.HasMany(c => c.Students)
                  .WithOne(s => s.Consultation);
        });
    }
}
