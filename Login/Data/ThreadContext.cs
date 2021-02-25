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
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //make the List Table
            modelBuilder.Entity<Thread>()
                .ToTable("Thread")
                .HasMany(thread => thread.LikedBy)
                .WithOne(like => like.Thread);

            //make the report table
            modelBuilder.Entity<Thread>()
                .ToTable("Thread")
                .HasMany(t => t.Reports)
                .WithOne(report => report.Thread);
        }
    }
}
