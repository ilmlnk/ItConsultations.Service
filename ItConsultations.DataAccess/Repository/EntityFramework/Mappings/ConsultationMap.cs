using ItConsultations.Business.Entities.Consultation;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItConsultations.DataAccess.Repository.EntityFramework.Mappings;

public class ConsultationMap
{
    public static void Map(EntityTypeBuilder<Consultation> entityBuilder)
    {
        entityBuilder.Property(t => t.Id).ValueGeneratedNever();
        entityBuilder.Property(t => t.ConsId);
    }
}
