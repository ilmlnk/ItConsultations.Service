using ItConsultations.Business.Entities.Consultation;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ItConsultations.DataAccess.Repository.EntityFramework.Mappings;

public class ConsultationMap
{
    public static void Map(EntityTypeBuilder<Consultation> entityBuilder)
    {
        entityBuilder.HasKey(c => c.Id);
        entityBuilder.Property(c => c.Id).ValueGeneratedNever();
        entityBuilder.Property(c => c.ConsId).HasMaxLength(32).IsRequired();
        entityBuilder.Property(c => c.Title).HasMaxLength(200).IsRequired();
        entityBuilder.Property(c => c.Description).HasMaxLength(2000);
        entityBuilder.Property(c => c.Price).HasPrecision(10, 2).IsRequired();
        entityBuilder.Property(c => c.Duration);
        entityBuilder.Property(c => c.ThumbnailUrl).HasMaxLength(500);
        
        // Настройка отношений
        entityBuilder.HasOne(c => c.Coach)
                     .WithMany(c => c.Consultations)
                     .HasForeignKey("CoachId")
                     .OnDelete(DeleteBehavior.Restrict);
        
        entityBuilder.HasMany(c => c.Students)
                     .WithOne(s => s.Consultation)
                     .HasForeignKey("ConsultationId")
                     .OnDelete(DeleteBehavior.Restrict);
        
        // Индексы для улучшения производительности
        entityBuilder.HasIndex(c => c.ConsId).IsUnique();
        entityBuilder.HasIndex(c => c.Title);
        entityBuilder.HasIndex(c => c.Price);
    }
}
