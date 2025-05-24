using ItConsultations.Business.Entities.Consultation;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItConsultations.DataAccess.Repository.EntityFramework.Mappings;

public class StudentMap
{
    public static void Map(EntityTypeBuilder<Student> entityBuilder)
    {
        entityBuilder.Property(t => t.Id).ValueGeneratedNever();
        entityBuilder.Property(t => t.StudentConsId);
    }
}
