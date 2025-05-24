using ItConsultations.Business.Entities.Consultation;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItConsultations.DataAccess.Repository.EntityFramework.Mappings;

public class CoachMap
{
    public static void Map(EntityTypeBuilder<Coach> entityBuilder)
    {
        entityBuilder.Property(t => t.Id).ValueGeneratedNever();
        entityBuilder.Property(t => t.CoachConsId);
    }
}
