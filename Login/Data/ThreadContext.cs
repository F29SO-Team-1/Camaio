using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Login.Models;
using Login.Data;
using Microsoft.AspNetCore.Http;

namespace Login.Data
{
    public class ThreadContext : DbContext
    {
        public ThreadContext(DbContextOptions<ThreadContext> options) : base(options)
        {
        }
        public DbSet<Thread> Threads { get; set; }
        public DbSet<Likes> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Thread>()
                .ToTable("Thread")
                .HasMany(thread => thread.LikedBy)
                .WithOne(like => like.Thread);
            //modelBuilder.Entity<Thread>().ToTable("Likes");
        }
    }
}
