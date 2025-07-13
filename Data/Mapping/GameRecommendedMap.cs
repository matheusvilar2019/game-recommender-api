using GameRecommenderAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameRecommenderAPI.Data.Mapping
{
    public class GameRecommendedMap : IEntityTypeConfiguration<GameRecommended>
    {
        public void Configure(EntityTypeBuilder<GameRecommended> builder)
        {
            // Table
            builder.ToTable("GameRecommended");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Identity
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            // Properties
            builder.Property(x => x.Title)
                .IsRequired()
                .HasColumnName("Title")
                .HasColumnType("VARCHAR")
                .HasMaxLength(50);

            builder.Property(x => x.Category)
                .IsRequired()
                .HasColumnName("Category")
                .HasColumnType("VARCHAR")
                .HasMaxLength(30);

            builder.Property(x => x.Counter)
                .IsRequired()
                .HasColumnName("Counter")
                .HasColumnType("INT")
                .HasMaxLength(8);
        }
    }
}
