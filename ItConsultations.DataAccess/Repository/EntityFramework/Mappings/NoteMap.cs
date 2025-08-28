using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ItConsultations.Business.Entities.Notes;

namespace ItConsultations.DataAccess.Repository.EntityFramework.Mappings;

public class NoteMap : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> entityBuilder)
    {
        entityBuilder.ToTable("Notes");
        
        entityBuilder.HasKey(n => n.Id);
        entityBuilder.Property(n => n.Id).ValueGeneratedOnAdd();
        entityBuilder.Property(n => n.NoteConsId).HasMaxLength(36).IsRequired();
        entityBuilder.Property(n => n.Title).HasMaxLength(500).IsRequired();
        entityBuilder.Property(n => n.Content).IsRequired();
        entityBuilder.Property(n => n.Type).IsRequired();
        entityBuilder.Property(n => n.Visibility).IsRequired();
        entityBuilder.Property(n => n.Priority).IsRequired();
        entityBuilder.Property(n => n.Status).IsRequired();
        entityBuilder.Property(n => n.AuthorId).IsRequired();
        entityBuilder.Property(n => n.ConsultationId);
        entityBuilder.Property(n => n.CoachId);
        entityBuilder.Property(n => n.StudentId);
        entityBuilder.Property(n => n.Tags).HasConversion(
            v => string.Join(',', v),
            v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
        );
        entityBuilder.Property(n => n.Location).HasMaxLength(200);
        entityBuilder.Property(n => n.Source).HasMaxLength(100);
        entityBuilder.Property(n => n.ScheduledFor);
        entityBuilder.Property(n => n.IsPinned).IsRequired();
        entityBuilder.Property(n => n.ViewCount).IsRequired();
        entityBuilder.Property(n => n.LastViewedAt);
        entityBuilder.Property(n => n.CreatedAt).IsRequired();
        entityBuilder.Property(n => n.UpdatedAt).IsRequired();
        entityBuilder.Property(n => n.DeletedAt);
        
        entityBuilder.HasOne(n => n.Consultation)
                     .WithMany()
                     .HasForeignKey(n => n.ConsultationId)
                     .OnDelete(DeleteBehavior.SetNull);
        
        entityBuilder.HasOne(n => n.Coach)
                     .WithMany()
                     .HasForeignKey(n => n.CoachId)
                     .OnDelete(DeleteBehavior.SetNull);
        
        entityBuilder.HasOne(n => n.Student)
                     .WithMany()
                     .HasForeignKey(n => n.StudentId)
                     .OnDelete(DeleteBehavior.SetNull);
        
        entityBuilder.HasOne(n => n.Author)
                     .WithMany()
                     .HasForeignKey(n => n.AuthorId)
                     .OnDelete(DeleteBehavior.Restrict);
        
        /*entityBuilder.HasIndex(n => n.NoteConsId).IsUnique();
        entityBuilder.HasIndex(n => n.Title);
        entityBuilder.HasIndex(n => n.Type);
        entityBuilder.HasIndex(n => n.Visibility);
        entityBuilder.HasIndex(n => n.Priority);
        entityBuilder.HasIndex(n => n.Status);
        entityBuilder.HasIndex(n => n.AuthorId);
        entityBuilder.HasIndex(n => n.ConsultationId);
        entityBuilder.HasIndex(n => n.CoachId);
        entityBuilder.HasIndex(n => n.StudentId);
        entityBuilder.HasIndex(n => n.CreatedAt);
        entityBuilder.HasIndex(n => n.UpdatedAt);
        entityBuilder.HasIndex(n => n.IsPinned);
        entityBuilder.HasIndex(n => n.ScheduledFor);
        entityBuilder.HasIndex(n => n.DeletedAt);*/
    }
} 