using Microsoft.EntityFrameworkCore;
using TicketManagement.Entities.Identity;
using TicketManagement.Entities.Tables;

namespace TicketManagement.DataAccess.Repositories.EntityFramework
{
    internal class TicketManagementContext : DbContext
    {
        public TicketManagementContext(DbContextOptions<TicketManagementContext> options)
            : base(options)
        {
        }

        public DbSet<Area> Area { get; set; }

        public DbSet<Event> Event { get; set; }

        public DbSet<EventArea> EventArea { get; set; }

        public DbSet<EventSeat> EventSeat { get; set; }

        public DbSet<Layout> Layout { get; set; }

        public DbSet<Seat> Seat { get; set; }

        public DbSet<Ticket> Ticket { get; set; }

        public DbSet<Venue> Venue { get; set; }

        public DbSet<User> AspNetUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Event>().HasNoKey();
        }
    }
}
