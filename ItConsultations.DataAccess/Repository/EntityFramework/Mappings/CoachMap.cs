using ItConsultations.Business.Entities.Coaches;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace ItConsultations.DataAccess.Repository.EntityFramework.Mappings;

public class CoachMap : IEntityTypeConfiguration<Coach>
{
    public void Configure(EntityTypeBuilder<Coach> builder)
    {
        builder.ToTable("Coaches");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();
        builder.Property(c => c.CoachConsId).HasMaxLength(36).IsRequired();
        builder.Property(c => c.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(c => c.LastName).HasMaxLength(100);
        builder.Property(c => c.BirthDate);
        builder.Property(c => c.Description).HasMaxLength(1000);
        builder.Property(c => c.Email).HasMaxLength(255).IsRequired();
        builder.Property(c => c.PictureUrl).HasMaxLength(500);
        builder.Property(c => c.LinkedInUrl).HasMaxLength(500);
        builder.Property(c => c.GitHubUrl).HasMaxLength(500);
        builder.Property(c => c.TelegramUrl).HasMaxLength(255);
        builder.Property(c => c.AverageRating).HasPrecision(3, 2);
        
        builder.Property(c => c.Skills)
            .HasDefaultValueSql("'{}'"); 

        builder.Property(c => c.Topics)
            .HasDefaultValueSql("'{}'");
        
        builder.HasMany(c => c.Consultations)
               .WithOne(c => c.Coach)
               .HasForeignKey("CoachId")
               .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(c => c.Reviews)
               .WithOne()
               .HasForeignKey("CoachId")
               .OnDelete(DeleteBehavior.Restrict);

        /*builder.HasOne(c => c.User)
               .WithOne()
               .HasForeignKey<Business.Entities.Users.UserEntity>(u => u.CoachId)
               .OnDelete(DeleteBehavior.Cascade);*/
        
        /*builder.HasIndex(c => c.CoachConsId).IsUnique();
        builder.HasIndex(c => c.Email).IsUnique();*/
    }
}
