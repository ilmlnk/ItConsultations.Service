using ItConsultations.Business.Entities.Article;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItConsultations.DataAccess.Repository.EntityFramework.Mappings;

public class ArticleMap
{
    public static void Map(EntityTypeBuilder<Article> entityBuilder)
    {
        entityBuilder.Property(t => t.Id).ValueGeneratedNever();
        entityBuilder.Property(t => t.ArticleConsId);
    }
}
