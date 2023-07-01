using System.ComponentModel.DataAnnotations;

namespace Tweet.API.Entities
{
    public class UserTweet
    {
        [Key]
        public int Id { get; set; } // Primary key property

        // Foreign key property for User
        public int UserId { get; set; }
        public User User { get; set; }

        // Foreign key property for Tweet
        public int TweetId { get; set; }
        public Tweet Tweet { get; set; }

    }

    public class UserLikedTweets
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int TweetId { get; set; }
        public Tweet Tweet { get; set; }
    }

    public class UserRetweetedTweets
    {
        [Key]
        public int Id { get; set; } // Primary key property

        // Foreign key property for User
        public int UserId { get; set; }
        public User User { get; set; }

        // Foreign key property for Tweet
        public int TweetId { get; set; }
        public Tweet Tweet { get; set; }
    }

}
