﻿using Login.Models;
using Microsoft.EntityFrameworkCore;

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

        }
    }
}
