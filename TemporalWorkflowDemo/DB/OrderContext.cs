using Microsoft.EntityFrameworkCore;
using TemporalWorkflowDemo.Models;

namespace TemporalWorkflowDemo.DB
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
        }

        public DbSet<Orders> Orders { get; set; }
        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Orders>()
                .Property(o => o.Status)
                .HasConversion<string>();
        }
    }
}
