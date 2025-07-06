using ItConsultations.Business.Entities.Consultation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItConsultations.DataAccess.Repository.EntityFramework.Mappings;

public class StudentMap : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedOnAdd();

        builder.Property(s => s.StudentConsId)
            .IsRequired()
            .HasMaxLength(36);

        builder.Property(s => s.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.LastName)
            .HasMaxLength(100);

        builder.Property(s => s.BirthDate);

        builder.Property(s => s.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(s => s.PictureUrl)
            .HasMaxLength(500);

        builder.Property(s => s.GitHubUrl)
            .HasMaxLength(255);

        builder.Property(s => s.LinkedInUrl)
            .HasMaxLength(255);

        builder.HasOne(s => s.Consultation)
            .WithMany(c => c.Students)
            .HasForeignKey("ConsultationId")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(s => s.User);

        builder.HasIndex(s => s.StudentConsId).IsUnique();
        builder.HasIndex(s => s.Email).IsUnique();
    }
}
