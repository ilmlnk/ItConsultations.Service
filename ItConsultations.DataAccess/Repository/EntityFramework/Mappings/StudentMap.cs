using ItConsultations.Business.Entities.Consultation;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ItConsultations.DataAccess.Repository.EntityFramework.Mappings;

public class StudentMap : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students");

        // builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedOnAdd();
        builder.Property(s => s.StudentConsId).HasMaxLength(36).IsRequired();
        builder.Property(s => s.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(s => s.LastName).HasMaxLength(100);
        builder.Property(s => s.BirthDate);
        builder.Property(s => s.Email).HasMaxLength(255).IsRequired();
        builder.Property(s => s.PictureUrl).HasMaxLength(500);
        builder.Property(s => s.GitHubUrl).HasMaxLength(500);
        builder.Property(s => s.LinkedInUrl).HasMaxLength(500);
        
        builder.HasOne(s => s.Consultation)
               .WithMany(c => c.Students)
               .HasForeignKey("ConsultationId")
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.User)
               .WithOne()
               .HasForeignKey<ItConsultations.Business.Entities.User.User>(u => u.StudentId)
               .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(s => s.StudentConsId).IsUnique();
        builder.HasIndex(s => s.Email).IsUnique();
    }
}
