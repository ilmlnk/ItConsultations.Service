using ItConsultations.Business.Entities.Article;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ItConsultations.DataAccess.Repository.EntityFramework.Mappings;

public class ArticleMap
{
    public static void Map(EntityTypeBuilder<Article> entityBuilder)
    {
        entityBuilder.HasKey(a => a.Id);
        entityBuilder.Property(a => a.Id).ValueGeneratedNever();
        entityBuilder.Property(a => a.ArticleConsId).HasMaxLength(32).IsRequired();
        entityBuilder.Property(a => a.Title).HasMaxLength(500).IsRequired();
        entityBuilder.Property(a => a.Text).IsRequired();
        entityBuilder.Property(a => a.CreatedAt).IsRequired();
        entityBuilder.Property(a => a.UpdatedAt).IsRequired();
        
        // Configure the relationship with Attachments
        entityBuilder.HasMany(a => a.Attachments)
                     .WithOne()
                     .HasForeignKey("ArticleId")
                     .OnDelete(DeleteBehavior.Cascade);
    }
}
