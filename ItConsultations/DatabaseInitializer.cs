using ItConsultations.DataAccess.Repository.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ItConsultations.Configuration;

public static class DatabaseInitializer
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ConsultationsDbContext>();

        try
        {
            if (!context.Database.CanConnect())
            {
                context.Database.EnsureCreated();
            }
            else
            {
                var pendingMigrations = context.Database.GetPendingMigrations().ToList();
                if (pendingMigrations.Count > 0)
                {
                    context.Database.Migrate();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database initialization error: {ex.Message}");
            throw;
        }
    }
}
