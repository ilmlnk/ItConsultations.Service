using ItConsultations.Business.Entities.LogEntry;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItConsultations.DataAccess.Repository.EntityFramework.Mappings;

public class LogEntryMap : IEntityTypeConfiguration<LogEntry>
{
    public void Configure(EntityTypeBuilder<LogEntry> builder)
    {
        builder.ToTable("LogEntries");

        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Timestamp)
            .IsRequired();

        builder.Property(x => x.LogLevel)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Message)
            .IsRequired()
            .HasMaxLength(4000);

        builder.Property(x => x.Exception)
            .HasMaxLength(4000);

        builder.Property(x => x.Source)
            .HasMaxLength(100);

        builder.Property(x => x.StackTrace)
            .HasMaxLength(4000);

        builder.Property(x => x.UserId)
            .HasMaxLength(100);

        builder.Property(x => x.SessionId)
            .HasMaxLength(100);

        builder.Property(x => x.RequestId)
            .HasMaxLength(100);

        builder.HasIndex(x => x.Timestamp);
        builder.HasIndex(x => x.LogLevel);
        builder.HasIndex(x => x.Source);
        builder.HasIndex(x => x.UserId);
    }
} 