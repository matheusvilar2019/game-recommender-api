using GameRecommenderAPI.Data.Mapping;
using GameRecommenderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GameRecommenderAPI.Data
{
    public class GameRecommenderDataContext : DbContext
    {
        public DbSet<GameRecommended> Games { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Server=localhost;Database=GameRecommender;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GameRecommendedMap());
        }
    }
}
