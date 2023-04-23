using Microsoft.EntityFrameworkCore;
using Tweet.API.Entities;

namespace Tweet.API.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tweet.API.Entities.Tweet>().HasKey(t => t.Id); // Define Id as the primary key

            modelBuilder.Entity<Tweet.API.Entities.Tweet>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tweets)
                .HasForeignKey(t => t.UserId);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Tweet.API.Entities.Tweet> Tweets { get; set; }

    }
}
