using ItConsultations.Business.Entities.Conferences;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItConsultations.DataAccess.Repository.EntityFramework.Mappings;

public class ConferenceMap : IEntityTypeConfiguration<Conference>
{
    public void Configure(EntityTypeBuilder<Conference> builder)
    {
        builder.ToTable("Conferences");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();

        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Description)
            .HasMaxLength(2000);

        builder.Property(c => c.StartTime)
            .IsRequired();

        builder.Property(c => c.EndTime);

        builder.Property(c => c.ConferenceUrl)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(c => c.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(c => c.IsRecordingEnabled)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(c => c.IsChatRecordingEnabled)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(c => c.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(c => c.UpdatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.HasOne(c => c.Organizer)
            .WithMany()
            .HasForeignKey(c => c.OrganizerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Consultation)
            .WithMany()
            .HasForeignKey(c => c.ConsultationConsId)
            .HasPrincipalKey(cons => cons.ConsId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(c => c.Participants)
            .WithOne(p => p.Conference)
            .HasForeignKey(p => p.ConferenceConsId)
            .HasPrincipalKey(p => p.ConferenceConsId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Notes)
            .WithOne()
            .HasForeignKey(n => n.ConferenceId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(c => c.Recordings)
            .WithOne(r => r.Conference)
            .HasForeignKey(r => r.ConferenceRecordingConsId)
            .OnDelete(DeleteBehavior.Cascade);

        /*builder.HasIndex(c => c.OrganizerId);
        builder.HasIndex(c => c.ConsultationId);
        builder.HasIndex(c => c.Status);
        builder.HasIndex(c => c.StartTime);*/
        builder.HasIndex(c => c.ConferenceUrl).IsUnique();
    }
}
