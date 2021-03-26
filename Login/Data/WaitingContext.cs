using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Login.Models;
using Login.Areas.Identity.Data;
using Login.Data;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Login.Data
{
    public class WaitingContext : DbContext
    {
        public WaitingContext(DbContextOptions<WaitingContext> options) 
            : base(options)
        {
        }
        public DbSet<WaitingToJoin> Pending { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WaitingToJoin>().ToTable("WaitingToJoin");
            modelBuilder.Entity<WaitingToJoin>()
                .HasOne(pending => pending.Channel)
                .WithMany(channel => channel.PendingRequests);
        }
    }
}
