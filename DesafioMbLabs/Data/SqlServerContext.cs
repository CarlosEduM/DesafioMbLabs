using DesafioMbLabs.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioMbLabs.Data
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
    }
}
