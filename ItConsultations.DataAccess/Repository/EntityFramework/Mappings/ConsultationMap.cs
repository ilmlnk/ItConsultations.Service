using ItConsultations.Business.Entities.Consultations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ItConsultations.DataAccess.Repository.EntityFramework.Mappings;

public class ConsultationMap : IEntityTypeConfiguration<Consultation>
{
    public void Configure(EntityTypeBuilder<Consultation> entityBuilder)
    {
        entityBuilder.ToTable("Consultations");
        
        entityBuilder.HasKey(c => c.Id);
        entityBuilder.Property(c => c.Id).ValueGeneratedOnAdd();
        entityBuilder.Property(c => c.ConsId).HasMaxLength(36).IsRequired();
        entityBuilder.Property(c => c.Title).HasMaxLength(200).IsRequired();
        entityBuilder.Property(c => c.Description).HasMaxLength(2000);
        entityBuilder.Property(c => c.Price).HasPrecision(10, 2).IsRequired();
        entityBuilder.Property(c => c.Duration);
        entityBuilder.Property(c => c.ThumbnailUrl).HasMaxLength(500);
        
        entityBuilder.HasOne(c => c.Coach)
                     .WithMany(c => c.Consultations)
                     .HasForeignKey("CoachId")
                     .OnDelete(DeleteBehavior.Restrict);
        
        entityBuilder.HasMany(c => c.Students)
                     .WithOne(s => s.Consultation)
                     .HasForeignKey("ConsultationId")
                     .OnDelete(DeleteBehavior.Restrict);
        
        entityBuilder.HasIndex(c => c.ConsId).IsUnique();
        entityBuilder.HasIndex(c => c.Title);
        entityBuilder.HasIndex(c => c.Price);
    }
}
