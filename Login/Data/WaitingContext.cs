using Microsoft.EntityFrameworkCore;
using Login.Models;


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
