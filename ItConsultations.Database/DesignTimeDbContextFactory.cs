using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ItConsultations.DataAccess.Repository.EntityFramework;

namespace ItConsultations.Database;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ConsultationsDbContext>
{
    public ConsultationsDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ConsultationsDbContext>();
        optionsBuilder.UseSqlServer("Server=VICTUS\\SQLEXPRESS;Database=ItConsultationsDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true");

        return new ConsultationsDbContext(optionsBuilder.Options);
    }
} 