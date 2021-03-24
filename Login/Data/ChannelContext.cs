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
    public class ChannelContext : DbContext
    {
        public ChannelContext(DbContextOptions<ChannelContext> options) 
            : base(options)
        {
        }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<ChannelMember> ChannelMember { get; set; }
        public DbSet<Album> Albums { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Channel>().ToTable("Channel");
            modelBuilder.Entity<ChannelMember>().ToTable("ChannelMember");
            modelBuilder.Entity<Album>().ToTable("Album");
            modelBuilder.Entity<ChannelMember>()
                .HasOne(cm => cm.Channel)
                .WithMany(c => c.ChannelMembers);
            modelBuilder.Entity<Album>()
                .HasOne(album => album.Channel)
                .WithMany(channel => channel.Albums);
            modelBuilder.Entity<Album>()
                .HasMany(album => album.Threads)
                .WithOne(thread => thread.Album);
        }
    }
}
