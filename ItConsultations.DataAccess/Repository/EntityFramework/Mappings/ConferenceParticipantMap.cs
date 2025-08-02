using ItConsultations.Business.Entities.Conferences;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItConsultations.DataAccess.Repository.EntityFramework.Mappings;

public class ConferenceParticipantMap : IEntityTypeConfiguration<ConferenceParticipant>
{
    public void Configure(EntityTypeBuilder<ConferenceParticipant> builder)
    {
        builder.ToTable("ConferenceParticipants");

        builder.HasKey(cp => cp.Id);
        builder.Property(cp => cp.Id).ValueGeneratedOnAdd();

        builder.Property(cp => cp.ConferenceConsId)
            .IsRequired();

        builder.Property(cp => cp.UserId)
            .IsRequired();

        builder.Property(cp => cp.Role)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(cp => cp.JoinedAt);

        builder.Property(cp => cp.LeftAt);

        builder.HasOne(cp => cp.Conference)
            .WithMany(c => c.Participants)
            .HasForeignKey(cp => cp.ConferenceConsId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cp => cp.User)
            .WithMany()
            .HasForeignKey(cp => cp.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(cp => cp.ConferenceConsId);
        builder.HasIndex(cp => cp.UserId);
        builder.HasIndex(cp => cp.Role);
        builder.HasIndex(cp => cp.JoinedAt);

        // Unique index to prevent duplicate participants
        builder.HasIndex(cp => new { cp.ConferenceConsId, cp.UserId }).IsUnique();
    }
}
