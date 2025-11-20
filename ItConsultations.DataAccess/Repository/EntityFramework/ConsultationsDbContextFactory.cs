using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ItConsultations.DataAccess.Repository.EntityFramework;

public class ConsultationsDbContextFactory : IDesignTimeDbContextFactory<ConsultationsDbContext>
{
    public ConsultationsDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "ItConsultations");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ConsultationsDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseNpgsql(connectionString, b => b.MigrationsAssembly("ItConsultations.Database"));
        return new ConsultationsDbContext(optionsBuilder.Options);
    }
}

// to create new migration use this command:
// cd ItConsultations.Database
// dotnet ef migrations add InitialCreate --startup-project ..\ItConsultations --context ConsultationsDbContext

// or

// from path ..\ItConsultations.Service: dotnet ef migrations add NotMappedAverageRating --project ItConsultations.Database --startup-project .\ItConsultations\ItConsultations.WebApi.csproj
// to update database use this command: dotnet ef database update --project ItConsultations.Database --startup-project .\ItConsultations\ItConsultations.WebApi.csproj