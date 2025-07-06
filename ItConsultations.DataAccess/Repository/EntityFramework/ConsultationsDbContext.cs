using ItConsultations.Business.Entities.Article;
using ItConsultations.Business.Entities.Attachments;
using ItConsultations.Business.Entities.Consultation;
using ItConsultations.Business.Entities.Event;
using ItConsultations.Business.Entities.LogEntry;
using ItConsultations.Business.Entities.User;
using ItConsultations.DataAccess.Repository.EntityFramework.Mappings;
using Microsoft.EntityFrameworkCore;

namespace ItConsultations.DataAccess.Repository.EntityFramework;

public class ConsultationsDbContext : DbContext
{
    public ConsultationsDbContext(DbContextOptions<ConsultationsDbContext> options) : base(options) { }

    public DbSet<Article> Articles { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<Coach> Coaches { get; set; }
    public DbSet<Consultation> Consultations { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<LogEntry> LogEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ArticleMap());
        modelBuilder.ApplyConfiguration(new CoachMap());
        modelBuilder.ApplyConfiguration(new ConsultationMap());
        modelBuilder.ApplyConfiguration(new StudentMap());
        modelBuilder.ApplyConfiguration(new UserMap());
        modelBuilder.ApplyConfiguration(new RefreshTokenMap());
        modelBuilder.ApplyConfiguration(new LogEntryMap());

        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.ToTable("Attachments");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id).IsRequired();
            entity.Property(a => a.AttachmentConsId).HasMaxLength(36).IsRequired();
            entity.Property(a => a.Name).IsRequired().HasMaxLength(255);
            entity.Property(a => a.FileName).IsRequired().HasMaxLength(255);
            entity.Property(a => a.CreatedAt).IsRequired();
            entity.Property(a => a.EntityId);
            entity.Ignore(a => a.EntityName);
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.ToTable("Reviews");
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Id).IsRequired();
            entity.Property(r => r.ReviewConsId).HasMaxLength(36).IsRequired();
            entity.Property(r => r.Text).HasMaxLength(2000);
            entity.Property(r => r.Rating).IsRequired();
            entity.Property(r => r.CreatedAt).IsRequired();
            entity.Property(r => r.UpdatedAt).IsRequired();

            entity.HasOne(r => r.Reviewer)
                  .WithMany()
                  .HasForeignKey("ReviewerId")
                  .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasMany(r => r.Attachments)
                  .WithOne()
                  .HasForeignKey("ReviewId")
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.ToTable("Events");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).IsRequired();
            entity.Property(e => e.EventConsId).HasMaxLength(36).IsRequired();
            entity.Property(e => e.Title).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.BeginDateTime).IsRequired();
            entity.Property(e => e.EndDateTime).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();

            entity.HasOne(e => e.Creator)
                  .WithMany()
                  .HasForeignKey("CreatorId")
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=VICTUS\\SQLEXPRESS;Database=ItConsultationsDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true",
                b => b.MigrationsAssembly("ItConsultations.Database"));
        }
    }
}
