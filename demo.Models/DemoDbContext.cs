using Demo.Models;
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
        public DbSet<GoogleLogin> googleLogins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GoogleLogin>(entity =>
            {
                entity.ToTable(nameof(GoogleLogin).Underscore());
                entity.HasKey(x => x.Id);
            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable(nameof(User).Underscore());
                entity.HasKey(k => k.Id);
                entity.HasOne<GoogleLogin>(g => g.GoogleLogin)
                .WithOne(u => u.User)
                .HasForeignKey<GoogleLogin>(f => f.UserId);
            });
        }
    }
}