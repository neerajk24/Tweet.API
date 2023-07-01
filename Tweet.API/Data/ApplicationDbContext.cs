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

            modelBuilder.Entity<Entities.Tweet>()
        .HasOne(t => t.User)
        .WithMany(u => u.Tweets)
        .HasForeignKey(t => t.UserId)
        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Entities.Tweet>()
                .HasMany(t => t.Comments)
                .WithOne(c => c.Tweet)
                .HasForeignKey(c => c.TweetId)
                .OnDelete(DeleteBehavior.Cascade);

    //        modelBuilder.Entity<Comment>()
    //.HasOne(c => c.User)
    //.WithMany()
    //.HasForeignKey(c => c.UserId);

    //        modelBuilder.Entity<Comment>()
    //.HasOne(c => c.User)
    //.WithMany()
    //.HasForeignKey(c => c.UserId)
    //.OnDelete(DeleteBehavior.Restrict);


            //modelBuilder.Entity<UserLikedTweets>()
            //.HasKey(ul => new { ul.UserId, ul.TweetId }); // Configure the composite primary key for the junction table

            //modelBuilder.Entity<UserLikedTweets>()
            //    .HasOne<User>(sc => sc.User)
            //    .WithMany(u => u.UserLikedTweets)
            //    .HasForeignKey(ul => ul.UserId); // Configure the foreign key relationship for Student

            //modelBuilder.Entity<UserLikedTweets>()
            //    .HasOne(ul => ul.Tweet)
            //    .WithMany(t => t.UserLikedTweets)
            //    .HasForeignKey(ul => ul.TweetId); // Configure the foreign key relationship for Course

            // modelBuilder.Entity<UserRetweetedTweets>()
            //.HasKey(ul => new { ul.UserId, ul.TweetId }); // Configure the composite primary key for the junction table

            // modelBuilder.Entity<UserRetweetedTweets>()
            //     .HasOne<User>(sc => sc.User)
            //     .WithMany(u => u.UserRetweetedTweets)
            //     .HasForeignKey(ul => ul.UserId); // Configure the foreign key relationship for Student

            // modelBuilder.Entity<UserRetweetedTweets>()
            //     .HasOne(ul => ul.Tweet)
            //     .WithMany(t => t.UserRetweetedTweets)
            //     .HasForeignKey(ul => ul.TweetId); // Configure the foreign key relationship for Course

            // ... other configurations ...

            modelBuilder.Entity<User>().HasKey(u => u.Id); // Define Id as the primary key for User entity

            modelBuilder.Entity<User>()
                .HasMany(u => u.Tweets) // User has many Tweets
                .WithOne(t => t.User) // Tweet has one User
                .HasForeignKey(t => t.UserId); // Set UserId as foreign key for Tweets

            modelBuilder.Entity<Entities.Tweet>().HasKey(t => t.Id); // Define Id as the primary key for Tweet entity

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Entities.Tweet> Tweets { get; set; }
        public DbSet<Comment> Comments { get; set; }


        //public DbSet<UserLikedTweets> UserLikedTweets { get; set; }
    }
}
