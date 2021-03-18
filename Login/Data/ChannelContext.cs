using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Login.Models;
using Login.Data;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Login.Data
{
    public class ChannelContext : DbContext
    {
        public ChannelContext(DbContextOptions<ChannelContext> options) 
            : base(options)
        {
        }
        public DbSet<Channel> Channel { get; set; }
        public DbSet<ChannelMember> ChannelMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Channel>().ToTable("Channel");
            modelBuilder.Entity<ChannelMember>().ToTable("ChannelMember");
            // modelBuilder.Entity<ChannelMember>()
            //     .HasKey(cm => new { cm.ChannelId, cm.UserId });  
            modelBuilder.Entity<ChannelMember>()
                .HasOne(cm => cm.Channel)
                .WithMany(c => c.ChannelMembers);
            modelBuilder.Entity<ChannelMember>()
                .HasOne(cm => cm.User)
                .WithMany(pm => pm.Channels);
        }
    }
}
