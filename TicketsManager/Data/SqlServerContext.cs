using TicketsManager.Models;
using Microsoft.EntityFrameworkCore;

namespace TicketsManager.Data
{
    public class SqlServerContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<EventManager> EventManagers { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<PaymentForm> PaymentForms { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public SqlServerContext(DbContextOptions<SqlServerContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<User>(nameof(User))
                .HasValue<EventManager>(nameof(EventManager));

            modelBuilder.Entity<User>()
                .Property("Discriminator")
                .HasMaxLength(64);

            modelBuilder.Entity<Ticket>()
                .HasOne(p => p.Owner)
                .WithMany(p => p.Tickets)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Ticket>()
                .HasOne(p => p.TicketEvent)
                .WithMany(p => p.Tickets)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
