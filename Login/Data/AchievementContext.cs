using Microsoft.EntityFrameworkCore;
using Login.Models;

namespace Login.Data
{
    public class AchievementContext : DbContext
    {
        public AchievementContext(DbContextOptions<AchievementContext> options) : base(options)
        {
        }
        public DbSet<Achievement> Achievement { get; set; }
        public DbSet<AchievementProgress> AchievementProgress { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //make the List Table
            /*modelBuilder.Entity<Achievement>()
                .ToTable("Achievement")
                .HasMany(x => x.AchievementProgress)
                .WithOne(x=>x.Achievement);*/

        }
    }
}
