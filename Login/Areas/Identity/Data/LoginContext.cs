using Login.Areas.Identity.Data;
using Login.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Login.Data
{
    public class LoginContext : IdentityDbContext<LoginUser>
    {
        public LoginContext(DbContextOptions<LoginContext> options)
            : base(options)
        {
        }

        public DbSet<Following> Follow { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            //this is for the list of users that follow a user 
            builder.Entity<LoginUser>()
               .ToTable("AspNetUsers")
               .HasMany(user => user.FollowsUser)
               .WithOne(following => following.FollowingUsers);
            builder.Entity<LoginUser>()
                .ToTable("AspNetUsers")
                .HasMany(user => user.Events)
                .WithOne(ep => ep.User);
            builder.Entity<LoginUser>()
                .ToTable("AspNetUsers")
                .HasMany(user => user.CreatedEvents)
                .WithOne(eventThing => eventThing.Creator);
            builder.Entity<LoginUser>()
                .ToTable("AspNetUsers")
                .HasMany(user => user.CreatedChannels)
                .WithOne(channel => channel.Creator);
            builder.Entity<LoginUser>()
                .ToTable("AspNetUsers")
                .HasMany(user => user.Channels)
                .WithOne(cm => cm.User);
            builder.Entity<LoginUser>()
                .ToTable("AspNetUsers")
                .HasMany(user => user.Notifications)
                .WithOne(n => n.User);
            builder.Entity<LoginUser>()
                .ToTable("AspNetUsers")
                .HasMany(user => user.PendingRequests)
                .WithOne(pending => pending.User);

        }
    }
}
