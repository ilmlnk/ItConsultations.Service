using ItConsultations.Business.Entities.Article;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ItConsultations.DataAccess.Repository.EntityFramework.Mappings;

public class ArticleMap : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> entityBuilder)
    {
        entityBuilder.ToTable("Articles");
        
        entityBuilder.HasKey(a => a.Id);
        entityBuilder.Property(a => a.Id).ValueGeneratedOnAdd();
        
        entityBuilder.Property(a => a.ArticleConsId)
            .HasMaxLength(36)
            .IsRequired();
            
        entityBuilder.Property(a => a.Title)
            .HasMaxLength(500)
            .IsRequired();
            
        entityBuilder.Property(a => a.Text)
            .IsRequired();
            
        entityBuilder.Property(a => a.CreatedAt)
            .IsRequired();
            
        entityBuilder.Property(a => a.UpdatedAt)
            .IsRequired();
        
        //entityBuilder.Ignore(a => a.EntityName);
        
        entityBuilder.HasOne(a => a.CreatedBy)
            .WithMany()
            .HasForeignKey("CreatedById")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
        
        // Configure the relationship with Attachments
        entityBuilder.HasMany(a => a.Attachments)
                     .WithOne()
                     .HasForeignKey("ArticleId")
                     .OnDelete(DeleteBehavior.Cascade);
                     
        entityBuilder.HasIndex(a => a.ArticleConsId).IsUnique();
        entityBuilder.HasIndex(a => a.Title);
        entityBuilder.HasIndex(a => a.CreatedAt);
    }
}
