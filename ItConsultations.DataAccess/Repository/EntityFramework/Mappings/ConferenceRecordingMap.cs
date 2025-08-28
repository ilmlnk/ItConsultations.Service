using ItConsultations.Business.Entities.Conferences;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItConsultations.DataAccess.Repository.EntityFramework.Mappings
{
    public class ConferenceRecordingMap : IEntityTypeConfiguration<ConferenceRecording>
    {
        public void Configure(EntityTypeBuilder<ConferenceRecording> builder)
        {
            builder.ToTable("ConferenceRecordings");

            builder.HasKey(cr => cr.Id);
            builder.Property(cr => cr.Id).ValueGeneratedOnAdd();

            builder.Property(cr => cr.ConferenceRecordingConsId)
                .IsRequired();

            builder.Property(cr => cr.RecordingUrl)
                .HasMaxLength(500);

            builder.Property(cr => cr.ChatLogUrl)
                .HasMaxLength(500);

            builder.Property(cr => cr.StartedAt);

            builder.Property(cr => cr.EndedAt);

            builder.Property(cr => cr.IsActive)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(cr => cr.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW()");

            builder.HasOne(cr => cr.Conference)
                .WithMany(c => c.Recordings)
                .HasForeignKey(cr => cr.ConferenceRecordingConsId)
                .HasPrincipalKey(cr => cr.ConferenceConsId)
                .OnDelete(DeleteBehavior.Cascade);

            /*builder.HasIndex(cr => cr.ConferenceRecordingConsId);
            builder.HasIndex(cr => cr.IsActive);*/
            builder.HasIndex(cr => cr.CreatedAt);
        }
    }
} 