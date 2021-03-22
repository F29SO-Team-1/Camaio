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
    public class EventContext : DbContext
    {
        public EventContext(DbContextOptions<EventContext> options) 
            : base(options)
        {
        }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventParticipant> Participants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().ToTable("Event");
            modelBuilder.Entity<EventParticipant>().ToTable("EventParticipant");
            modelBuilder.Entity<EventParticipant>()
                .HasOne(ep => ep.Event)
                .WithMany(eventThing => eventThing.Participants);
        }
    }
}
