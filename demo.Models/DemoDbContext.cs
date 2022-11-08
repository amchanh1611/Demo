using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace demo.Models
{
    public class DemoDbContext : DbContext
    {
        public DemoDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable(nameof(User).Underscore());
                entity.HasKey(k => k.Id);
            });
        }
    }
}