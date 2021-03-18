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
    public class NotificationContext : DbContext
    {
        public NotificationContext(DbContextOptions<NotificationContext> options) 
            : base(options)
        {
        }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>().ToTable("Notification");
            modelBuilder.Entity<Event>()
                .ToTable("Event")
                .HasMany(eventThing => eventThing.Notifications)
                .WithOne(n => n.Event);
            modelBuilder.Entity<Channel>()
                .ToTable("Channel")
                .HasMany(channel => channel.Notifications)
                .WithOne(n => n.Channel);
            modelBuilder.Entity<Thread>()
                .ToTable("Thread")
                .HasMany(thread => thread.Notifications)
                .WithOne(n => n.Thread);
        }
    }
}
