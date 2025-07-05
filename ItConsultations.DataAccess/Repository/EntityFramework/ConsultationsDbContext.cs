using ItConsultations.Business.Entities.Article;
using ItConsultations.Business.Entities.Attachments;
using ItConsultations.Business.Entities.Consultation;
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
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //modelBuilder.ApplyConfiguration(new ArticleMap());
        modelBuilder.ApplyConfiguration(new CoachMap());
        //modelBuilder.ApplyConfiguration(new ConsultationMap());
        modelBuilder.ApplyConfiguration(new StudentMap());
        modelBuilder.ApplyConfiguration(new UserMap());
        modelBuilder.ApplyConfiguration(new RefreshTokenMap());
        
        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.ToTable("Attachments");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id).IsRequired();
            entity.Property(a => a.Name).IsRequired().HasMaxLength(255);
            entity.Property(a => a.FileName).IsRequired().HasMaxLength(255);
            entity.Property(a => a.CreatedAt).IsRequired();
            entity.Property(a => a.EntityId);
            entity.Property(a => a.EntityName);
            entity.Property(a => a.Id).HasMaxLength(32);
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.ToTable("Reviews");
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Id).IsRequired();
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
    }
}
