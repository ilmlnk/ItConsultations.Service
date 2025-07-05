using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ItConsultations.DataAccess.Repository.EntityFramework;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ConsultationsDbContext>
{
    public ConsultationsDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ConsultationsDbContext>();
        
        var connectionString = "Server=VICTUS\\SQLEXPRESS;Database=ItConsultationsDB;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true";
        
        optionsBuilder.UseSqlServer(connectionString);

        return new ConsultationsDbContext(optionsBuilder.Options);
    }
} 