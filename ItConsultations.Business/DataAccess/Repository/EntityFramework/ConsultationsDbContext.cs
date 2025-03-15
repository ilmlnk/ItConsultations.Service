using Microsoft.EntityFrameworkCore;

namespace ItConsultations.Business.DataAccess.Repository.EntityFramework;

public class ConsultationsDbContext : DbContext
{
    public ConsultationsDbContext(DbContextOptions<ConsultationsDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    }
}
