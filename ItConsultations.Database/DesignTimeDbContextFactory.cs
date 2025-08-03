using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ItConsultations.DataAccess.Repository.EntityFramework;

namespace ItConsultations.Database;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ConsultationsDbContext>
{
    public ConsultationsDbContext CreateDbContext(string[] args)
    {
        throw new NotImplementedException();
    }
} 