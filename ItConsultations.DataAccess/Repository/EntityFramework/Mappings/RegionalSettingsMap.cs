using ItConsultations.Business.Entities.RegionalSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItConsultations.DataAccess.Repository.EntityFramework.Mappings;

public class LanguageMap : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.ToTable("Languages");
        
        builder.HasKey(x => x.Id);
        builder.Property(c => c.Id)
            .HasMaxLength(10)
            .ValueGeneratedNever(); 

        builder.Property(c => c.DisplayName)
            .IsRequired()
            .HasMaxLength(100);
    }
}

public class CultureMap : IEntityTypeConfiguration<Culture>
{
    public void Configure(EntityTypeBuilder<Culture> builder)
    {
        builder.ToTable("Cultures");
        
        builder.HasKey(x => x.Id);
        builder.Property(c => c.Id)
            .HasMaxLength(10)
            .ValueGeneratedNever(); 

        builder.Property(c => c.DisplayName)
            .IsRequired()
            .HasMaxLength(100);
    }
}

public class CurrencyMap : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.ToTable("Currencies");
        
        builder.HasKey(x => x.Id);
        builder.Property(c => c.Id)
            .HasMaxLength(10)
            .ValueGeneratedNever(); 

        builder.Property(c => c.DisplayName)
            .IsRequired()
            .HasMaxLength(100);
    }
}

public class CountryMap : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("Countries");
        
        builder.HasKey(x => x.Id);
        builder.Property(c => c.Id)
            .HasMaxLength(10)
            .ValueGeneratedNever(); 

        builder.Property(c => c.DisplayName)
            .IsRequired()
            .HasMaxLength(100);
    }
}