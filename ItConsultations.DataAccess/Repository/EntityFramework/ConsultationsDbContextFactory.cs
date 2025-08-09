using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ItConsultations.DataAccess.Repository.EntityFramework;

public class ConsultationsDbContextFactory : IDesignTimeDbContextFactory<ConsultationsDbContext>
{
    public ConsultationsDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ConsultationsDbContext>();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        optionsBuilder.UseNpgsql(connectionString, b => b.MigrationsAssembly("ItConsultations.Database"));

        return new ConsultationsDbContext(optionsBuilder.Options);
    }
}